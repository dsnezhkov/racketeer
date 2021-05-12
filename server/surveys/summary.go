package surveys

import "RNS/types"

func SummarySurvey(task *types.TaskCommandRequestOutbound) error {
	task.TaskType = 2
	task.TaskName = "summary"
	task.TaskArgs = []string{}
	task.TaskB64Payload = ""
	return nil
}
