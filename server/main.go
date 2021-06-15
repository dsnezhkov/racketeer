// +build windows linux

package main

import (
	"RNS/serve"
	"RNS/types"
	"embed"
	"flag"
)
//go:embed keys/* man/*
var static embed.FS

func main() {

	hostP := flag.String("H", "127.0.0.1", "IP listen to")
	portP := flag.String("P", "3000", "IP listen to") // We dont' bother with conversion from int, http will handle
	secureP := flag.Bool("s", true, "TLS enabled")

	flag.Parse()

	if *secureP == true {
		serve.StartHttpServer(types.Secure, hostP, portP)
	}else {
		serve.StartHttpServer(types.Plain, hostP, portP)
	}

	serve.TuiShell()
}


