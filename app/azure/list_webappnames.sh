

function print_appname {
  echo $1: $(pulumi stack output WebappName -s $1)
}

print_appname demo
print_appname dev
print_appname mosh
print_appname upstreet
print_appname upstreet-dev
print_appname vitable
