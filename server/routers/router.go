package routers

import (
	"RNS/types"
	"RNS/utils"
	"encoding/json"
	"fmt"
	"github.com/go-chi/chi"
	"github.com/tidwall/pretty"
	"net/http"
	"time"
)

func CommsRouter(rtr *chi.Mux,
	agentCommandCh *map[string]chan types.TaskCommandRequestOutbound,
	agentPendingList *map[string]types.PendingAgent,
	agentActiveStats *map[string]types.AgentStats) {

	var err error
	var taskCommandOut types.TaskCommandRequestOutbound
	(*rtr).Get("/task/{siteId}", func(response http.ResponseWriter, request *http.Request) {

		siteId := chi.URLParam(request, "siteId")
		if _, ok := (*agentCommandCh)[siteId]; !ok {
			// siteChannel does not exists. Adding to pending list
			if _, ok := (*agentPendingList)[siteId]; !ok {
				// only add unique channel
				(*agentPendingList)[siteId] = types.PendingAgent{}
			}
		}

		t :=  time.Now()
		if _, ok := (*agentPendingList)[siteId]; ok {
			agent := (*agentPendingList)[siteId]
			agent.CheckIn = t.Local()
			(*agentPendingList)[siteId] = agent
		}
		if _, ok := (*agentActiveStats)[siteId]; ok {
			agent := (*agentActiveStats)[siteId]
			agent.CheckIn = t.Local()
			(*agentActiveStats)[siteId] = agent
		}

		taskCommandOut = types.TaskCommandRequestOutbound{}
		select {
		case userCommand := <- (*agentCommandCh)[siteId]:
			// fmt.Println("<= ", userCommand)
			err = json.NewEncoder(response).Encode(userCommand)
			if err != nil {
				println("Error : {-1}", err)
				response.WriteHeader(http.StatusInternalServerError)
			}
		default:
			err = json.NewEncoder(response).Encode(taskCommandOut)
			if err != nil {
				println("Error : {-1}", err)
				response.WriteHeader(http.StatusInternalServerError)
			}
			// fmt.Println("no message received")
		}

		response.Header().Set("Content-Type", "application/json")
	})

	(*rtr).Post("/heartbeat", func(response http.ResponseWriter, request *http.Request) {
		println("=> heartbeat ")
		var heartbeatCommand types.HeartbeatCommandResponse
		var taskCommandResponseInbound types.TaskCommandResponseInbound
		err = json.NewDecoder(request.Body).Decode(&taskCommandResponseInbound)
		if err != nil {
			response.WriteHeader(http.StatusBadRequest)
		}
		err = json.Unmarshal([]byte(taskCommandResponseInbound.TaskMessage), &heartbeatCommand)
		if err != nil {
			response.WriteHeader(http.StatusBadRequest)
		}

		fmt.Printf("[%s]\n", taskCommandResponseInbound.SiteId)
		fmt.Printf("\tHost: %s, PID: %d (A:%t), User: %s\n",
			heartbeatCommand.ProcessHost,
			heartbeatCommand.ProcessPid, heartbeatCommand.IsAdministrator, heartbeatCommand.ProcessUser)
		response.WriteHeader(http.StatusOK)
	})
	(*rtr).Post("/summary", func(response http.ResponseWriter, request *http.Request) {
		println("=> summary ")
		var summaryCommand types.SummaryCommandResponse
		var taskCommandResponseInbound types.TaskCommandResponseInbound

		err = json.NewDecoder(request.Body).Decode(&taskCommandResponseInbound)
		if err != nil {
			fmt.Println("Error response decoder:", err)
			response.WriteHeader(http.StatusBadRequest)
		}

		fmt.Printf("[%s]\n", taskCommandResponseInbound.SiteId)
		switch taskCommandResponseInbound.TaskStatus {
		case types.Success:
			b64decoded, err := utils.StrB642Str(taskCommandResponseInbound.TaskB64Payload)
			if err != nil {
				fmt.Printf("Error decoding task payload (b64): %+v", err)
				break
			}

			err = json.Unmarshal([]byte(b64decoded), &summaryCommand)
			if err != nil {
				fmt.Printf("Error response unmarshal : %+v", err)
				response.WriteHeader(http.StatusBadRequest)
			}

			fmt.Printf("Execution Report for Site ID %s\n", summaryCommand.SiteId)
			fmt.Println(string(pretty.Pretty([]byte(b64decoded))))

			// for hts := range summaryCommand.HostTaskSummaries {
			// }
		case types.Fail:
			fmt.Printf("Policy execution failed\n")
		case types.Pending:
			fmt.Printf("Policy has not been executed.\n")
		default:
			fmt.Printf("Unknown status code received %d\n", taskCommandResponseInbound.TaskStatus)
		}
		response.WriteHeader(http.StatusOK)
	})

	(*rtr).Post("/getlogs", func(response http.ResponseWriter, request *http.Request) {
		println("=> getlogs ")
		var taskCommandResponseInbound types.TaskCommandResponseInbound

		err = json.NewDecoder(request.Body).Decode(&taskCommandResponseInbound)
		if err != nil {
			fmt.Println("Error response decoder:", err)
			response.WriteHeader(http.StatusBadRequest)
		}

		fmt.Printf("[%s]\n", taskCommandResponseInbound.SiteId)
		switch taskCommandResponseInbound.TaskStatus {
		case types.Success:
			b64decoded, err := utils.StrB642Str(taskCommandResponseInbound.TaskB64Payload)
			if err != nil {
				fmt.Printf("Error decoding task payload (b64): %+v", err)
				break
			}
			fmt.Println(b64decoded)
		default:
			break
		}
		response.WriteHeader(http.StatusOK)
	})
}
