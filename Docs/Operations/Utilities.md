## TODO:
 - Json policy file linter / schema verifier
 - Web UI portal 
 - Finish IPC client 

## KeyGen
This is used to manage keys, generate site id, and help in the offline file decryption


Usage:
```txt
.\FileConnectorKeyGen.exe --help
FileConnectorKeyGen 1.0.0.0
Copyright c  2021

  site         Generate Site ID

  masterkey    Generate master key for site

  filekey      Generate one or more of file keys

  credkey      Secure cred storage management

  file         Secure file management

  help         Display more information on a specific command.

  version      Display version information.
```

### SiteId generation
Command:
```
.\FileConnectorKeyGen.exe site 
```
Output:
```
2e1bb2f0-c20a-4eb6-baa4-18832af621c20
```
### Master Key Hash generation
Command:
```
.\FileConnectorKeyGen.exe  masterkey -s 2e1bb2f0-c20a-4eb6-baa4-18832af621c2d
Enter master key: *****
```
Output:
```
$RNS$V1$10000$8LIbLgrCtk66pBiDKvYhwpFwvzpYKz6jWaUGh9kV89jIJwOLF
```

### Credential shielding
To shield:

Command:
```
.\FileConnectorKeyGen.exe credkey  -E -s 2e1bb2f0-c20a-4eb6-baa4-18832af621c2 -m $RNS$V1$10000$8LIbLgrCtk66pBiDKvYhwpFwvzpYKz6jWaUGh9kV89jIJwOL

```
Output:
```
Enter master key: *****
2e1bb2f0-c20a-4eb6-baa4-18832af621c2
Provided MK hash: $RNS$V1$10000$8LIbLgrCtk66pBiDKvYhwpFwvzpYKz6jWaUGh9kV89jIJwOL
Computed MK hash: $RNS$V1$10000$8LIbLgrCtk66pBiDKvYhwpFwvzpYKz6jWaUGh9kV89jIJwOL
Enter cred key to encrypt: ********
Answer: PV2c2Iu2x4HwRUtxh9kKxQ==
```

To unshield:

Command:
```
.\FileConnectorKeyGen.exe credkey  -D -s 2e1bb2f0-c20a-4eb6-baa4-18832af621c2 -m $RNS$V1$10000$8LIbLgrCtk66pBiDKvYhwpFwvzpYKz6jWaUGh9kV89jIJwOL -e PV2c2Iu2x4HwRUtxh9kKxQ==
```

Output:
```
Enter master key: *****
Provided MK hash: $RNS$V0$10000$8LIbLgrCtk66pBiDKvYhwpFwvzpYKz6jWaUGh9kV89jIJwOL
Computed MK hash: $RNS$V0$10000$8LIbLgrCtk66pBiDKvYhwpFwvzpYKz6jWaUGh9kV89jIJwOL
Answer: password
```

### File Key generation 

Command:
```
.\FileConnectorKeyGen.exe filekey
```

Output:
```
aee65c4b-c416-43a5-89c3-0bce130a7af8
```

Or: 

Command:
```
.\FileConnectorKeyGen.exe filekey -c 6
```

Output:
```
06a7f99c-6f89-4ec3-a100-a390612f1e0f
b52709de-e4d4-4659-83aa-a88f6c6e874c
d4d20f58-b83e-4084-8f50-eb20e7ac0740
072d6c83-28bf-4bb9-b02a-5be7d6ccf4ff
f2910044-1a80-4c6a-a68b-88326f4ae913
7c6c67dc-6f73-4987-99c5-4680c4f26d32
```

## Encrypt by Rules


```
FileConnector.exe
Enter master password: *****
2e1bb2f0-c20a-4eb6-baa4-18832af621c2
10110-LT-X0874, \Users\dimsne01\Downloads\Share\trdock_debug.log, Encrypt, 5fd3c359-c58a-4c11-8cd0-f4c4695be28f
Encrypting \\10110-LT-X0874\Users\dimsne01\Downloads\Share\trdock_debug.log -> \\10110-LT-X0874\Users\dimsne01\Downloads\Share\trdock_debug.log.enc

5fd3c359-c58a-4c11-8cd0-f4c4695be28f
Derived Key: EA-99-88-DF-84-4E-F8-0B-38-1D-C5-1E-D9-A0-71-FB-41-A2-04-B6-A7-EF-65-53-64-8B-ED-03-FD-E4-7B-DE
Wrote salt CC-77-D4-D8-8C-D4-2D-61-F3-DD-F5-4C-88-BC-98-0E-0B-6D-A5-6A-D5-0A-B6-D2-20-66-EE-56-F4-CB-C4-77 of size 32
Read size from src file : 1002
Wrote size to dest file : 1002
Flushed
```
## Decrypt / Verify individual file(s)

Command: 

```
FileConnectorKeyGen.exe  file -D -i C:\Users\dimsne01\Downloads\Share\trdock_debug.log.enc -o  C:\Users\dimsne01\Downloads\Share\trdock_debug.log.dec -k 5fd3c359-c58a-4c11-8cd0-f4c4695be28f
```

Output:
```
Decrypting ...

5fd3c359-c58a-4c11-8cd0-f4c4695be28f
Read salt DE-C4-BF-3C-AF-78-25-6E-B2-92-A7-1C-00-28-84-63-EF-FF-0E-50-7F-12-F5-8F-75-D4-A6-21-9A-80-D9-3A of size 32
Derived Key: 89-B7-59-43-03-22-D7-82-59-47-04-77-14-06-08-EC-EF-D8-FC-35-1F-6C-81-B1-6A-E2-B1-92-8A-F3-93-C7
Read size from src file (enc) : 1008
Wrote size to dst  file (dec) : 1008
```