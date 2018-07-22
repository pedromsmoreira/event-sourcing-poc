# event-sourcing-samples
Event Sourcing Implementation Samples

[![CircleCI](https://circleci.com/gh/pedromsmoreira/event-sourcing-poc.svg?style=svg)](https://circleci.com/gh/pedromsmoreira/event-sourcing-poc)
[![CodeFactor](https://www.codefactor.io/repository/github/pedromsmoreira/event-sourcing-poc/badge)](https://www.codefactor.io/repository/github/pedromsmoreira/event-sourcing-poc)

This is a project that shows a simple implementation of Event Sourcing with MongoDb.

### Installing

``` bash
$ git clone https://github.com/pedromsmoreira/event-sourcing-poc.git
```

### Running in Visual Studio 2017

#### Pre requirements

* [Visual Studio 2017][vs2017]

### How to use it

``` bash
F5
```

### Running in Docker

#### Pre requirements

* [Docker Compose][docker_compose]

### How to use it

``` bash
$ docker-compose up
```

### Access the container

``` bash
http://localhost:5000/swagger/index.html
```

### Contributions

1. Fork it
2. git checkout -b <branch-name>
3. git add --all && git commit -m "feature description"
4. git push origin <branch-name>
5. Create a pull request

### License

The code is available under the [MIT license](LICENSE).

[vs2017]: https://www.visualstudio.com/vs/whatsnew/
[docker_compose]: https://docs.docker.com/compose/
[mongo db]: https://www.mongodb.com/
[ELK Stack]: https://www.elastic.co/