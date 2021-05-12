package surveys

import "RNS/types"

func ClearLogsSurvey(task *types.TaskCommandRequestOutbound) error {
	task.TaskType = 2
	task.TaskName = "clearlogs"
	task.TaskArgs = []string{}
	task.TaskB64Payload = ""
	return nil
}
