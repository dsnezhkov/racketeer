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

func StartHttpServer(serverType types.HttpServerType) {

	//Create the default router
	router := chi.NewRouter()
	router.Use(cors.Handler(cors.Options{
		AllowedOrigins: []string{"*"},
		AllowedMethods: []string{"GET", "POST"},
	}))
	routers.CommsRouter(router, &agentChannelsOut, &agentPendingList, &agentActiveStats)
	serverPlain := &http.Server{
		Addr:    "127.0.0.1:3000",
		Handler: router,
	}
	serverSecure := &http.Server{
		Addr:    "127.0.0.1:3001",
		Handler: router,
	}

	//Create the server.
	switch serverType {
	case types.Plain:
		go startHTTPServer(serverPlain)
	case types.Secure:
		go startHTTPSServer(serverSecure)
	case types.Both:
		go startHTTPServer(serverPlain)
		go startHTTPSServer(serverSecure)
	default:
		fmt.Println("Invalid HTTP server type")
	}

}

func startHTTPSServer(s *http.Server) {
	fmt.Println("Starting HTTPS Server")
	cert := filepath.FromSlash("dist/server.crt")
	key := filepath.FromSlash("dist/server.key")
	err := s.ListenAndServeTLS(cert, key)
	if err != nil {
		println("Error : {0}", err)
	}
}
func startHTTPServer(s *http.Server) {
	fmt.Println("Starting HTTP Server")
	err := s.ListenAndServe()
	if err != nil {
		println("Error : {0}", err)
	}
}
