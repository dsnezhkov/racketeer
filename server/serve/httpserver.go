package serve

import (
	"RNS/routers"
	"RNS/types"
	"fmt"
	"github.com/go-chi/chi"
	"github.com/go-chi/cors"
	"net/http"
	"path/filepath"
)


func StartHttpServer(serverType types.HttpServerType, hostP *string, portP *string) {

	//Create the default router
	router := chi.NewRouter()
	router.Use(cors.Handler(cors.Options{
		AllowedOrigins: []string{"*"},
		AllowedMethods: []string{"GET", "POST"},
	}))
	routers.CommsRouter(router, &agentChannelsOut, &agentPendingList, &agentActiveStats)

	agentServer := &http.Server{
		Addr:   *hostP + ":" + *portP,
		Handler: router,
	}

	//Create the server.
	switch serverType {
	case types.Plain:
		go startHTTPServer(agentServer)
	case types.Secure:
		go startHTTPSServer(agentServer)
	default:
		fmt.Println("Invalid HTTP server type")
	}

}

func startHTTPSServer(s *http.Server) {
	fmt.Println("Starting HTTP/S Server - " + s.Addr)
	cert := filepath.FromSlash("keys/server.crt")
	key := filepath.FromSlash("keys/server.key")
	err := s.ListenAndServeTLS(cert, key)
	if err != nil {
		println("Error : ", err)
	}
}
func startHTTPServer(s *http.Server) {
	fmt.Println("Starting HTTP Server - " + s.Addr)
	err := s.ListenAndServe()
	if err != nil {
		println("Error : {0}", err)
	}
}
