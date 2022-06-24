@echo off
title Automated Environment Planner
color 0A

ECHO login with AZ login? (y/n) default = n
set /p choice=""

IF /i "%choice%" == "y" GOTO yes
IF /i "%choice%" == "ye" GOTO yes
IF /i "%choice%" == "yes" GOTO yes
IF /i "%choice%" == "ys" GOTO yes
IF /i "%choice%" == "" goto no
IF /i "%choice%" == "n" GOTO no
IF /i "%choice%" == "no" GOTO no

ECHO Erroneous input
pause
goto end

:yes
az login
Terraform -help || ECHO Please make sure that Terraform is installed! Otherwise, just drop the .exe in the local folder and rerun the .bat file timeout 
Terraform init 
Terraform plan
GOTO end

:no
Terraform -help || ECHO Please make sure that Terraform is installed! Otherwise, just drop the .exe in the local folder and rerun the .bat file timeout 
Terraform init 
Terraform plan
goto end

:end
pause
Exit /B n
