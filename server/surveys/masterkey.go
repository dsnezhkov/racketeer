package surveys

import (
	"RNS/help"
	"RNS/types"
	"RNS/utils"
	"fmt"
	"github.com/AlecAivazis/survey"
)

func MasterKeySurvey(task *types.TaskCommandRequestOutbound) error {

	var payload string
	var err error

	masterKey := ""
	masterKeyQ := &survey.Password{
		Message: "Please type your masterkey",
		Help: help.MasterKeyHelp(),
	}

	err = survey.AskOne(masterKeyQ, &masterKey)
	if err != nil {
		return err
	}
	fmt.Println("masterkey: ", masterKey)
	payload, err = utils.Str2B64Str(masterKey)
	if err != nil {
		return err
	}

	fmt.Println("Encoded masterkey: ", payload)

	approveA := false
	approveQ := &survey.Confirm{
		Message: "OK to send?",
	}
	_ = survey.AskOne(approveQ, &approveA)

	if approveA == true {
		task.TaskType = 2
		task.TaskName = "masterkey"
		task.TaskArgs = []string{}
		task.TaskB64Payload = payload
	}
	return nil
}
