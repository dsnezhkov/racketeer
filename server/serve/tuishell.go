package serve

import (
	"RNS/surveys"
	"RNS/types"
	"RNS/utils"
	"fmt"
	"github.com/c-bata/go-prompt"
	"os"
	"strings"
)

// --------------------- //
// var userCommandCh = make(chan types.TaskCommandRequestOutbound)
var defaultAgent = "no agent"
var agent = defaultAgent
var agentChannelsOut = make(map[string]chan types.TaskCommandRequestOutbound)
var agentPendingList = make(map[string]types.PendingAgent)
var agentActiveStats = make(map[string]types.AgentStats)

// - User command processing
func send(ch chan<- types.TaskCommandRequestOutbound, userCommand *types.TaskCommandRequestOutbound) {
	ch <- *userCommand
}

// - Prompts
func executor(in string) {

	var task *types.TaskCommandRequestOutbound
	in = strings.TrimSpace(in)

	types.LivePrefixState.LivePrefix =  agent  + ":" + in + " > "
	types.LivePrefixState.IsEnable = true
	if in == "" {
		types.LivePrefixState.LivePrefix =  agent  + ":" + " > "
		return
	} else if in == "quit" || in == "exit" {
		// Close command channels
		for siteId, siteChannel := range agentChannelsOut {
			fmt.Printf("Closing channel to %s\n", siteId)
			close(siteChannel)
		}
		fmt.Println("Out")
		// signal clean exit
		os.Exit(0)
		return
	}

	commandArgs := strings.Split(in, " ")
	promptName := commandArgs[0]
	switch promptName {
	case "activate":
		manageActiveAgent(commandArgs)
	case "heartbeat":
		if ok := assertAgentCtx(); ok {
			task = new(types.TaskCommandRequestOutbound)
			task.SiteId = agent
			checkTaskToUserChan(task, surveys.HeartbeatSurvey(task))
		}
	case "masterkey":
		if ok := assertAgentCtx(); ok {
			task = new(types.TaskCommandRequestOutbound)
			task.SiteId = agent
			checkTaskToUserChan(task, surveys.MasterKeySurvey(task))
		}
	case "summary":
		if ok := assertAgentCtx(); ok {
			task = new(types.TaskCommandRequestOutbound)
			task.SiteId = agent
			checkTaskToUserChan(task, surveys.SummarySurvey(task))
		}
	case "polexec":
		if ok := assertAgentCtx(); ok {
			task = new(types.TaskCommandRequestOutbound)
			task.SiteId = agent
			checkTaskToUserChan(task, surveys.PolExecSurvey(task))
		}
	case "agent":
		if ok := assertAgentCtx(); ok {
			if len(commandArgs) >= 2 {
				switch commandArgs[1] {
				case "self":
					if len(commandArgs) >= 3 {
						switch commandArgs[2] {
						case "unhide":
							if len(commandArgs) >= 4 {
								switch commandArgs[3] {
								case "console":
									task = new(types.TaskCommandRequestOutbound)
									task.SiteId = agent
									fmt.Println("Unhiding console ... ")
									checkTaskToUserChan(task, surveys.ExecAgentSurvey(task, types.UnhideConsole ))
								case "message":
									task = new(types.TaskCommandRequestOutbound)
									task.SiteId = agent
									fmt.Println("Unhiding message ... ")
									checkTaskToUserChan(task, surveys.ExecAgentSurvey(task, types.UnhideMessage ))
								default:
									println("no such command")
								}
							}
						case "terminate":
							task = new(types.TaskCommandRequestOutbound)
							task.SiteId = agent
							fmt.Println("Terminating Agent ... ")
							checkTaskToUserChan(task, surveys.ExecAgentSurvey(task, types.SelfTerminate ))
						default:
							println("no such command")
						}
					}
				default:
					println("no such command")
				}
			}
		}
	case "logs":
		if ok := assertAgentCtx(); ok {
			if len(commandArgs) >= 2 {
				switch commandArgs[1] {
				case "get":
					 task = new(types.TaskCommandRequestOutbound)
					 task.SiteId = agent
					 fmt.Println("Getting logs ... ")
					 // commandName := strings.Join( []string{promptName,commandArgs[1]},"")
					 checkTaskToUserChan(task, surveys.GetLogsSurvey(task))
				case "clear":
					 task = new(types.TaskCommandRequestOutbound)
					 task.SiteId = agent
					 fmt.Println("Clearing logs ... ")
					 checkTaskToUserChan(task, surveys.ClearLogsSurvey(task))
				case "source":
					if len(commandArgs) == 3 {
						switch commandArgs[2] {
						case "console":
							task = new(types.TaskCommandRequestOutbound)
							task.SiteId = agent
							fmt.Println("Setting logging source ... ")
							checkTaskToUserChan(task, surveys.SetLogsSourceSurvey(task, types.Console))
						case "memory":
							task = new(types.TaskCommandRequestOutbound)
							task.SiteId = agent
							fmt.Println("Setting logging source ... ")
							checkTaskToUserChan(task, surveys.SetLogsSourceSurvey(task, types.Memory))
						default:
							println("no such command")

						}
					}
				case "set":
					if len(commandArgs) == 3 {
						switch commandArgs[2] {
						case "debug":
							task = new(types.TaskCommandRequestOutbound)
							task.SiteId = agent
							fmt.Println("Setting logging level ... ")
							checkTaskToUserChan(task, surveys.SetLogsSurvey(task, types.Debug))
						case "info":
							task = new(types.TaskCommandRequestOutbound)
							task.SiteId = agent
							fmt.Println("Setting logging level ... ")
							checkTaskToUserChan(task, surveys.SetLogsSurvey(task, types.Info))
						case "warn":
							task = new(types.TaskCommandRequestOutbound)
							task.SiteId = agent
							fmt.Println("Setting logging level ... ")
							checkTaskToUserChan(task, surveys.SetLogsSurvey(task, types.Warn))
						case "error":
							task = new(types.TaskCommandRequestOutbound)
							task.SiteId = agent
							fmt.Println("Setting logging level ... ")
							checkTaskToUserChan(task, surveys.SetLogsSurvey(task, types.Error))
						default:
							println("no such command")
						}
					}
				default:
					println("no such command")
				}
			}
		}
	default:
		println("no such command")
	}
}
func assertAgentCtx() bool {
	if agent == defaultAgent {
		fmt.Println("Set active agent context first (hint: agent)")
		executor("")
		return false
	}
	return true
}
func manageActiveAgent(blocks []string){
	if len(blocks) > 1 {

		// Add new agent
		agent = strings.TrimSpace(blocks[1])

		// Deactivate agent context
		if agent == "none"{
			agent = defaultAgent
			return
		}

		if ok := utils.IsValidGUID(agent); !ok{
			fmt.Println("This does not seem like a valid agent id. Format: UUID")
			return
		}
		// New channel for agent, if does not exist
		println("Activating context ", agent)
		ch := make(chan types.TaskCommandRequestOutbound)
		// Save it into map
		agentChannelsOut[agent] = ch

		// Move checkin from pending to active,
		// Remove agent from pending list
		if _, ok := agentPendingList[agent]; ok {
			agentActiveStats[agent] = types.AgentStats{ CheckIn: agentPendingList[agent].CheckIn}
			delete(agentPendingList, agent)
		}

	}else{
		// List agents
		fmt.Println("Agent (active):")
		for siteId := range agentChannelsOut {
			fmt.Printf("%s %s\n", siteId,
				agentActiveStats[siteId].CheckIn)
		}
		fmt.Println("\nAgents (pending):")
		for siteId := range agentPendingList {
			fmt.Printf("%s %s\n", siteId,
				agentPendingList[siteId].CheckIn)
		}
	}
}
func resetLivePrefix() {
	types.LivePrefixState.LivePrefix = "> "
}
func checkTaskToUserChan(task *types.TaskCommandRequestOutbound, err error) {
	if err == nil {
		go send(agentChannelsOut[agent], task)
	} else {
		fmt.Printf("Error in survey: %v", err)
	}
	// resetLivePrefix()
}

func livePrefix() (string, bool) {
	return types.LivePrefixState.LivePrefix, types.LivePrefixState.IsEnable
}
func completer(in prompt.Document) []prompt.Suggest {
	tbc := in.TextBeforeCursor()
	args := strings.Split(tbc, " ")
	depth := len(args)

	if in.Text == ""{
		return []prompt.Suggest{}
	}
	switch depth {
	case 1:
		suggests := []prompt.Suggest{
			{Text: "logs", Description: "i would my shell to show all valid commands"},
			{Text: "activate", Description: "i would my shell to show all valid commands"},
			{Text: "agent", Description: "i would my shell to show all valid commands"},
			{Text: "heartbeat", Description: "i would my shell to show all valid commands"},
			{Text: "masterkey", Description: "i would my shell to show all valid commands"},
			{Text: "polexec", Description: "i would my shell to show all valid commands"},
			{Text: "summary", Description: "i would my shell to show all valid commands"},
		}
		return prompt.FilterHasPrefix(suggests, in.GetWordBeforeCursor(), true)
	case 2:
		switch args[0] {
		case "logs":
			suggests := []prompt.Suggest{
				{Text: "get", Description: "..."},
				{Text: "set", Description: "..."},
				{Text: "clear", Description: "..."},
				{Text: "source", Description: "..."},
			}
			return prompt.FilterHasPrefix(suggests, in.GetWordBeforeCursor(), true)
		case "agent":
			suggests := []prompt.Suggest{
				{Text: "self", Description: "..."},
			}
			return prompt.FilterHasPrefix(suggests, in.GetWordBeforeCursor(), true)
		}
	case 3:
		secondLevel := strings.Join(args, " ")
		secondLevel = strings.TrimSpace(secondLevel)
		// switch args[1] {
		switch secondLevel {
		case "logs set":
			suggests := []prompt.Suggest{
				{Text: "debug", Description: "..."},
				{Text: "info", Description: "..."},
				{Text: "warn", Description: "..."},
				{Text: "error", Description: "..."},
			}
			return suggests
		case "logs source":
			suggests := []prompt.Suggest{
				{Text: "console", Description: "..."},
				{Text: "memory", Description: "..."},
			}
			return suggests
		case "agent self":
			suggests := []prompt.Suggest{
				{Text: "unhide", Description: "..."},
				{Text: "terminate", Description: "..."},
			}
			return suggests
		default:
			// fmt.Printf("Secondlevel: (%s)\n", secondLevel)
		}
	case 4:
		thirdLevel := strings.Join(args, " ")
		thirdLevel = strings.TrimSpace(thirdLevel)
		switch thirdLevel {
		case "agent self unhide":
			suggests := []prompt.Suggest{
				{Text: "console", Description: "..."},
				{Text: "message", Description: "..."},
			}
			return suggests
		case "agent self terminate":
		default:
		}

	}
	return []prompt.Suggest{}
}

func banner() string {
	b := `
  ____    _   _   ____  
 |  _ \  | \ | | / ___| 
 | |_) | |  \| | \___ \ 
 |  _ <  | |\  |  ___) |
 |_| \_\ |_| \_| |____/ 
 v0.5 Full Metal Jacket

`
	return b
}

func TuiShell() {

	fmt.Println(banner())
	p := prompt.New(
		executor,
		completer,
		prompt.OptionPrefix(agent + ":" + "> "),
		prompt.OptionLivePrefix(livePrefix),
		prompt.OptionTitle("RNS"),
	)
	p.Run()
}

