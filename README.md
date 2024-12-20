# RapidRabbitMQ v. 1.4.5
![image](https://github.com/cloudfy/rapidrabbitmq/assets/31371919/b15b773b-1930-4c75-b67a-39ca97a83237)
RabbitMQ portable. Enables you to run RabbitMQ on Windows without installing Erlang. The command line downloads and configure all dependencies.

## Getting started
1. Download the latest release from the releases. 
2. Extract the zip file and run `rapidrabbitmq.exe prepare`.
3. Run `rapidrabbitmq.exe run` to run.
4. Use `rapidrabbitmq.exe clean` to clean up.

## Management portal of RabbitMQ
Management portal of RabbitMQ is enabled by default. Access via `https://localhost:15671` or `http://localhost:15672`. Default username and password is `guest` and `guest`.

## RabbitMQ
localhost:5672 is default ready for RabbitMQ queues.

## Versions
| Version | Erlang | RabbitMQ |
| --- | --- | --- |
| 1.4 | 27.1 | 4.0.4 |
| 1.0 | 24.0 | 3.8.9 |

## Copyrights
[RabbitMQ](https://rabbitmq.com/) (Copyright © 2005-2023 Broadcom.) and [Erlang](https://www.erlang.org/) remains on commercial license. This project is just a wrapper to make it easier to run RabbitMQ on Windows 64bit.
