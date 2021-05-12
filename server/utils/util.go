package utils

import (
	"bytes"
	"encoding/base64"
	"io/ioutil"
	"os"
	"regexp"
	"text/template"
)

func Str2B64Str(payload string) (string, error) {
	encoded := base64.StdEncoding.EncodeToString([]byte(payload))
	return encoded, nil
}
func StrB642Str(b64payload string) (string, error) {
	decoded, err := base64.StdEncoding.DecodeString(b64payload)
	if err != nil {
		return "", err
	}
	return string(decoded), nil
}
func IsValidGUID(uuid string) bool {
	r := regexp.MustCompile("^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-4[a-fA-F0-9]{3}-[8|9|aA|bB][a-fA-F0-9]{3}-[a-fA-F0-9]{12}$")
	return r.MatchString(uuid)
}
func RunTemplate(vars interface{}, name string) (string, error) {

	var manPagesDir = "." + string(os.PathSeparator) +
		"man" + string(os.PathSeparator)

	helpTemplate, err := GetContentFile(
		manPagesDir + name + ".template")

	if err != nil{
		return "", err
	}

	var tmplBytes bytes.Buffer
	tmpl, err := template.New(name).Parse(helpTemplate)
	if err != nil {
		return "", err
	}
	err = tmpl.Execute(&tmplBytes, vars)
	if err != nil {
		return "", err
	}
	return tmplBytes.String(), nil
}

func GetContentFile(fileName string) (string, error) {

	content, err := ioutil.ReadFile(fileName)
	if err != nil {
		return "", err
	}
	return string(content), nil
}
