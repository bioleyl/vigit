## Migrations

Create a migration
```
dotnet ef migrations add [migration name f.e. AddSshKey]
```

Apply the migration
```
dotnet ef database update
```

## Test from host

in powershel, create a ssh key
```
ssh-keygen -t rsa -b 4096 -C "leal@yourdomain.com"
cat $HOME/.ssh/id_rsa.pub
```

> For the moment, add it manually into the DB

get the wsl ip
```bash
ip addr show eth0
```

from windows
```
git clone ssh://git@<WSL_IP>/test.git
```

# Vigit

## System Configuration

install git
```bash
sudo apt install git openssh-server
```

add a git user
```
sudo adduser --disabled-password --gecos "" git
```

create the folder for config files and add rights to it
```
sudo mkdir -p /opt/vigit/scripts
sudo chown root:root /opt/vigit/scripts
sudo chmod 775 /opt/vigit/scripts
```

> All the parent directories and the script should have root:root to make sshd work

create the file `/etc/ssh/sshd_config.d/git.conf`

```
# Dynamic key lookup for git user
Match User git
  AuthorizedKeysCommand /opt/vigit/scripts/authorized-keys %k
  AuthorizedKeysCommandUser git
  AllowTcpForwarding no
  X11Forwarding no
  PasswordAuthentication no
  PubkeyAuthentication yes
```
> The user "git" should match the user in GitSsh.User and the path "/tmp/vigit/scripts" should match the path in GitSsh.TargetScriptDir in the appsettings.json

And restart the sshd

```bash
sudo service ssh start
```
