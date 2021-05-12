package surveys

import (
	"RNS/types"
	"RNS/utils"
	"errors"
	"fmt"
	"github.com/AlecAivazis/survey"
	"io/ioutil"
	"path/filepath"
)

func PolExecSurvey(task *types.TaskCommandRequestOutbound) error {

	var payload string
	var err error

	payloadAS := ""
	payloadQS := &survey.Select{
		Message: "Choose source of payload:",
		Options: []string{"editor", "file"},
	}
	_ = survey.AskOne(payloadQS, &payloadAS)

	switch payloadAS {
	case "editor":
		payloadEA := ""
		payloadEQ := &survey.Editor{
			Message:  "Format: JSON policy",
			FileName: "*.txt",
		}

		_ = survey.AskOne(payloadEQ, &payloadEA)
		fmt.Printf("Content received: %s", payloadEA)
		if payloadEA == "" {
			payload = payloadEA
			return errors.New("empty payload")
		}
	case "file":
		payloadFA := ""
		payloadFQ := &survey.Input{
			Message: "file to get payload from:",
			Suggest: func(toComplete string) []string {
				files, _ := filepath.Glob(toComplete + "*")
				return files
			},
		}
		_ = survey.AskOne(payloadFQ, &payloadFA)
		fmt.Printf("Processing paylaod from file: %s\n", payloadFA)
		payloadFAC, err := ioutil.ReadFile(payloadFA)
		if err != nil {
			fmt.Printf("Error reading payload file: %v", err)
			return err
		}
		// convert bytes to string
		payload = string(payloadFAC)
	default:
	}

	// Encode payload
	payloadB64, err := utils.Str2B64Str(payload)
	if err != nil {
		return err
	}

	approveA := false
	approveQ := &survey.Confirm{
		Message: "OK to send?",
	}
	_ = survey.AskOne(approveQ, &approveA)

	if approveA == true {
		task.TaskType = 2
		task.TaskName = "polexec"
		task.TaskArgs = []string{}
		task.TaskB64Payload = payloadB64
	}
	return nil
}
