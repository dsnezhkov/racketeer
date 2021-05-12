package surveys

import "RNS/types"

func HeartbeatSurvey(task *types.TaskCommandRequestOutbound) error {
	task.TaskType = 2
	task.TaskName = "heartbeat"
	task.TaskArgs = []string{}
	task.TaskB64Payload = ""
	return nil
}
