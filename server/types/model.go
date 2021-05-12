package types

import "time"

type HttpServerType uint

const (
	Plain HttpServerType = iota
	Secure
	Both
)

type TaskStatus uint64
const (
	Success TaskStatus = iota
	Fail
	Progress
	Pending
)
type TaskType uint64

type TaskCommandRequestOutbound struct {
	SiteId        string   `json:"siteId"`
	TaskName       string   `json:"taskName"`
	TaskType       TaskType `json:"taskType"`
	TaskArgs       []string `json:"taskArgs"`
	TaskB64Payload string   `json:"taskB64Payload"`
}

type TaskCommandResponseInbound struct {
	SiteId         string     `json:"SiteId"`
	TaskName       string     `json:"taskName"`
	TaskStatus     TaskStatus `json:"taskStatus"`
	TaskMessage    string     `json:"taskMessage"`
	TaskB64Payload string     `json:"taskB64Payload"`
}

type HeartbeatCommandResponse struct {
	ProcessPid  uint   `json:"ProcessPid"`
	ProcessHost string `json:"ProcessHost"`
	ProcessUser string `json:"ProcessUser"`
	IsAdministrator bool `json:"isAdministrator"`
}

type Operation uint

const (
	None Operation = iota
	Encrypt
	Decrypt
)

type SummaryCommandResponse struct {
	SiteId            string            //`json:"SiteId"`
	HostTaskSummaries []HostTaskSummary `json:"HostTaskSummaries"`
}
type HostTaskSummary struct {
	HostIdent         string            `json:"HostIdent"`
	FileTaskSummaries []FileTaskSummary `json:"FileTaskSummaries"`
}
type FileTaskSummary struct {
	PreImageName  string    `json:"PreImageName"`
	PostImageName string    `json:"PostImageName"`
	PreImageHash  string    `json:"PreImageHash"`
	PostImageHash string    `json:"PostImageHash"`
	PreImageSize  int64     `json:"PreImageSize"`
	PostImageSize int64     `json:"PostImageSize"`
	ImageTime     string    `json:"ImageTime"`
	SymKey        string    `json:"SymKey"`
	Operation     Operation `json:"Operation"`
}

// Logging in agent
const (
	Debug uint64 = iota
	Info
	Warn
	Error
)
// Logging source in agent
const (
	Console uint64 = iota
	Memory
)
// Agent exec commands
const (
	UnhideConsole uint64 = iota
	UnhideMessage
	SelfTerminate
)

type PendingAgent struct {
	CheckIn time.Time
}
type AgentStats struct {
	Host string
	Pid int
	IsAdmin bool
	User string
	CheckIn time.Time
}