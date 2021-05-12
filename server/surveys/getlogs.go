package surveys

import "RNS/types"

func GetLogsSurvey(task *types.TaskCommandRequestOutbound) error {
	task.TaskType = 2
	task.TaskName = "getlogs"
	task.TaskArgs = []string{}
	task.TaskB64Payload = ""
	return nil
}
