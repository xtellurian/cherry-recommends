#!/bin/bash

az ad sp create-for-rbac -n "azure-devops-dev" \
--role Contributor --scopes /subscriptions/ad2a181b-b804-4179-904a-012445b7d1f5

