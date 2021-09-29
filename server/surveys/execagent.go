package surveys

import (
	"RNS/types"
	"errors"
	"github.com/AlecAivazis/survey/v2"
	"strconv"
)

func ExecAgentSurvey(task *types.TaskCommandRequestOutbound, etask uint64 ) error {
	task.TaskType = 2
	task.TaskName = "agentselfexec"
	task.TaskArgs = []string{ strconv.FormatUint(etask, 10)}
	task.TaskB64Payload = ""

	if etask == types.SelfTerminate {

		approveA := false
		approveQ := &survey.Confirm{
			Message: "Sure terminate agent? This is irreversible.",
		}
		_ = survey.AskOne(approveQ, &approveA, nil)

		if approveA == false {
			return errors.New("action cancelled")
		}
	}
	return nil
}
