@echo off
title Automated environment planner
Terraform -help || ECHO Please make sure that Terraform is installed! Otherwise, just drop the .exe in the local folder and rerun the .bat file timeout && pause 
Terraform init 
Terraform plan
pause