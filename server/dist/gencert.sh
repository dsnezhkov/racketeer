openssl genrsa -aes256 -passout pass:xxxx -out ca.pass.key 4096
openssl rsa -passin pass:xxxx -in ca.pass.key -out server.key
rm ca.pass.key

openssl req -nodes -new -x509 -days 3650 -key server.key -out server.crt \
-subj "/C=US/ST=Chicago/L=Chicago/O=Headquarters/OU=IT Department/CN=*.local"

