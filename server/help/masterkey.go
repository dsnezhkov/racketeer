package help

import (
	"RNS/types"
	"RNS/utils"
	"fmt"
)

func MasterKeyHelp() string{

	var mkh = types.CommandHelpContent{
		CommandType: "Runtime",
		OpSecTraits: "HTTP traffic",
	}

	output, err := utils.RunTemplate(mkh, "masterkey" )
	if err != nil {
		fmt.Println("error in help ", err)
		return ""
	}
	return output
}