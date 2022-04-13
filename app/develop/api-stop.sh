#!/bin/bash

# brute force way to kill it
kill -9 $(lsof -t -i:5001)