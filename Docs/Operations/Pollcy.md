## Policy File Format

```json
	"OpMode": 2,
	"ExtEndpoints": [
		{
            "Ident": "Base1",
			"Base": "localhost:3001",
			"Proto": "HTTP",
			"TLS": true,
			"Profile": {
				"Interval": 10,
			},
			"Timeout": 5,
		},
		{
            "Ident": "Base2",
			"Base": "localhost:3000",
			"Proto": "HTTP",
			"TLS": false,
			"Profile": {
				"Interval": 5,
			},
			"Timeout": 3,
		}
	],
	"ExtComm": {
			"As": "Base1",
			"Contracts": {
                    "task":{
						"Resource": "/task",
						"Request": { 
							"Method": 1
						}
					},
					"heartbeat":{
						"Resource": "/heartbeat",
						"Response": { 
							"Method": 2
						}
					},
					"summary":{
						"Resource": "/summary",
						"Response": { 
							"Method": 2
						}
					},
					"getlogs":{
						"Resource": "/getlogs",
						"Response": { 
							"Method": 2
						}
					}
			}
    },
    // Keys
	"Keys": {
        // master key 
		"MasterKey": "$RNS$V1$10000$8LIbLgrCtk66pBiDKvYhwpFwvzpYKz6jWaUGh9kV89jIJwOL",
        // site id
		"SiteID": "2e1bb2f0-c20a-4eb6-baa4-18832af621c2"
		},
    // Authentication credentials to match hosts in any permutations
	"AuthN": [ 
		{
      // Identifier used by hosts
      "Ident": "Acct1",
      // Type of authentication 
      // None:0, 
      // Local (no network logon, this host as logged in user) :1, 
      // NetworkImplicit (logged in user) :2
      // NetworkExplicit (with supplied network creds) :3

      "Type:": 1,
        // Cred triple
        "Domain": "NA.IT.COMPANY.COM", 
        "User": "user01", 
        // Password encoded by offline utility (KeyGen)
        "Password": "cJ959sGLNGtjsM+uyGXhLlWPhZMbGJ1xWNwt4HDSHos="
		},
		{
       "Ident": "Acct2",
			"Domain": "NA.IT.COMPANY.COM", 
			"User": "user01", 
			"Password": "cJ959sGLNGtjsM+uyGXhLlWPhZMbGJ1xWNwt4HDSHos="
		},
		{
      // Encrypting local files (scenario: dropper)
      "Ident": "Local",
			"Domain": "", 
			"User": "", 
			"Password": ""
		}
    ], 
    // Hosts to connect to and work with files. 
    "Hosts":[
		{
      // Host logical identifier
			"Ident": "HostA",
      // IP or DNS 
      "Name": "10110-PC-X0874", 
      // Which AuthN identifier to use to connect with 
      "As": "Local",
      // Encryption operation. None:0, Encrypt:1, Decrypt:2
      // This is designed to granualry enc/dec destination files in simulations
			"Operation": 1,
      // Key to use to encrypt all files on host unless overriden by file Key
			"Key": "f34febd0-2774-4829-96a6-aadbf5398ab3",
      // Files on host to work with
      "Files": [
          {
            // File Path.
            // Can be in the formats:
            // /path/to/file - this works well with JSON
            // \\path\\to\\file - double backslash 
            // C:/path/to/file - this works for local files
            // C:\\path\\to\\file - this works for local files
            "FilePath": "/Users/user01/Downloads/Share/file5.log", 
            // Encryption operation. None:0, Encrypt:1, Decrypt:2
            // 0: None will default to per-Host operation
						"Operation": 2,
            // Key to use to encrypt file at file level.
            // Can override the host.
            // To have host-level keys work, leave the value empty.
            "Key": "9993c359-c58a-4c11-8cd0-f4c4695be28f"
           },
           {
            "FilePath": "X:/file6.log", 
            "Operation": 1,
            "Key": "88888359-c58a-4c11-8cd0-f4c4695be28f"
            }
         ]
        },
        {
            "Ident": "HostB",
            "Name": "10110-PC-X0874", 
            "As": "Acct2",
            "Operation": 0,
            "Key": "7149e694-511c-4a89-b3b6-95372471f498",
            "Files": [
                {
                  "FilePath": "/Share/file1.log", 
                  "Operation": 1,
                  "Key": ""
                },
                {
                  "FilePath": "\\Share\\file2.log", 
                  "Operation": 1,
                  "Key": "6fd3c359-c58a-4c11-8cd0-f4c4695be28f"
                },
                {
                  "FilePath": "\\Share\\file4.log", 
                  "Operation": 1,
                  "Key": "6fd3c359-c58a-4c11-8cd0-f4c4695be28f"
                }
            ]
        },
		{
			"Ident": "HostC",
      "Name": "10110-PC-X0874", 
      "As": "Acct2",
			"Operation": 1,
			"Key": "f34febd0-2774-4829-96a6-aadbf5398ab3",
      "Files": [
            {
                "FilePath": "/Users/user01/Downloads/Share/file3.log", 
                "Operation": 2,
                "Key": ""
            }
            ]
        }
    ]
}
```
