package surveys

import (
	"RNS/types"
	"strconv"
)

func SetLogsSourceSurvey(task *types.TaskCommandRequestOutbound, level uint64 ) error {
	task.TaskType = 2
	task.TaskName = "setlogsource"
	task.TaskArgs = []string{ strconv.FormatUint(level, 10)}
	task.TaskB64Payload = ""
	return nil
}
