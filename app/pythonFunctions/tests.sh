#!/bin/bash

if [ -z "$CI" ]
then
    echo "Not CI - running normal tests"
    pytest tests 
else
    echo "CI - collecting results"
    pytest tests --doctest-modules --junitxml=junit/test-results.xml --cov=. --cov-report=xml
fi