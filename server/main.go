package main

import (
	"RNS/serve"
	"RNS/types"
)

func main() {
	serve.StartHttpServer(types.Both)
	serve.TuiShell()
}


