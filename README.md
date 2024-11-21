## Planejamento

**I. Elaborar base de dados: MongoDB** <br>
**II. Desenvolver Controller e Service** <br>
**III. Integração Backend e Banco de Dados (requisições)** <br>
**IV. Desenvolver Frontend** <br>

### Pensamentos

- A aplicação deve ser disponibilizável para utilização em outras máquinas sem dificuldades.
- Todos os métodos CRUD devem ser implementados.

### Detalhamento

- Para disponibilidade da aplicação, a base de dados será CONTAINERIZADA.

## Guia de utilização do Podman

**I. Para utilização de uma Máquina Virtual:**

% podman machine init <nome_da_maquina>
% podman machine start
% podman system connection default <nome_da_maquina> [Para garantir utilização da máquina especificada]

**II. Uma vez rodando, a máquina está pronta para criação do Container:**

% podman run --detach --name <nome_do_container> -p <porta_local>:<porta_container> docker.io/mongodb/mongodb-community-server:latest

- <porta_local>: Porta da máquina local que será mapeada para a porta do container (e.g. 3000 ou 27017)
- <porta_container>: Porta do container atribuída à porta local (e.g 27017)
- O último parâmetro consiste na imagem Docker na qual o container gerado se baseará (blueprint)

**III. Container criado, hora da execução da base:**

% podman exec -it <nome_do_container> mongosh

- Este comando causa a execução do mongosh, o que permite a interação com as bases de dados.
- Para segregação, criamos uma nova base onde as collections (no caso deste projeto, apenas uma) serão criadas

## Guia de utilização do Mongosh

**I. Primeiramente, criamos uma nova base de dados:**

/~ use <nome_da_base>

**II. Agora, para interação, é necessária uma Collection**

/~ db.createCollection("Tasks", {})

- O nome da coleção, passado no primeiro parâmetro, costuma ser no plural
- O segundo parâmetro corresponde às validações para as regras de documentos inseridos na coleção
- Os documentos passados na inserção devem ser em formato JSON ({"chave": "valor"})

```
# Exemplo de validação para documentos a serem INSERIDOS

db.createCollection("Tasks", {
validator: {

    $jsonSchema: {
      bsonType: "object",
      required: ["description", "completed"],
      properties: {
        description: {
          bsonType: "string",
          description: "Sentence to describe task."
        },
        completed: {
          bsonType: "bool",
          description: "Determine if task is completed, therefore displayed."
        },
      }
    }

},
validationLevel: "strict"
}
)
```

Para a interação da API com o Banco de Dados, utilizamos o seguinte artigo:
[Create a RESTful API With .NET Core and MongoDB](https://www.mongodb.com/developer/languages/csharp/create-restful-api-dotnet-core-mongodb/?msockid=058229b7cbb46b4c37143cb1ca056a7f)

What's a "singleton"?

### Guia de utilização do Podman-Compose

**Instalação**
Primeiramente, tentei a instalação através da aplicação Podman Desktop; O Setup persistia em retornar erros. Portanto, a maneira mais fácil que encontrei foi através do pip.

- Instala-se Python em: https://www.python.org/downloads
- Adicione o caminho do comando "pip" nas vars de ambiente:

  - C:\Users\bruno.concli\AppData\Local\Programs\Python\Python313\Scripts

- Para adicionar o caminho do comando "python" nas vars de ambiente (como dica):

  - C:\Users\bruno.concli\AppData\Local\Programs\Python\Python313

- Utilize o comando "pip install podman-compose" [Instalação_concluída]

**Utilização**

I. Cria-se um arquivo "docker-compose.yml", contendo as especificações do banco e da API

- Segundo exemplo presente já no código.

II. Cria-se o arquivo Dockerfile, contendo os passos de iniciação do projeto.

- Também segundo exemplo presente na aplicação.

> **Questões a saber:**
> Considerando a utilização de um sistema Windows ou Mac, é necessária a presença e execução de uma máquina virtual para o funcionamento dos containers.
> Teremos dois containers (por ora):
>
> - Um para o banco mongodb: Chamado "taskcli-container" neste projeto e construído a partir de uma imagem docker.io/mongodb:latest
> - E outro para a API: Chamado "taskcli-api-service", construído a partir de uma imagem criada manualmente através do comando "podman build -t taskcli_image_api_dotnet ."

II.i. Uma vez que:

- Criado o Dockerfile
- Criado o docker-compose.yml
- Começada a máquina virtual (e mantida em execução)
- Criada a imagem para a api (usando "podman build -t <nome_da_imagem> .")

É possível executar o comando **"podman-compose up -d"** [-d:Segundo-plano]

> **Possíveis problemas:**
> Ambos os containers devem ser subidos ao mesmo tempo: Se um dos containers falhar na execução do comando, o outro container poderá subir normalmente e terá de ser manualmente deletado para a execução do comando pela segunda vez.
>
> - Neste caso, os containers podem não subir por problemas na imagem.
>   Antes de uma segunda execução do comando _"podman-compose up -d"_, é necessário rodar o comando oposto, **podman-compose down** para garantir a parada de qualquer container ou pod ainda em execução.
> - Em todo caso, é possível parar manualmente a execução destes através dos comandos: "podman pod stop <nome_do_pod>" e "podman rm -f -a" [remover todos os containers em execução]

### Processo de inicialização da Aplicação

podman build -t <nome_da_imagem>:latest -f .\Dockerfile
podman run --name <nome_do_container> -p <porta_local>:<porta_exposta> localhost/<nome_da_imagem>:latest

podman rm --all

### Última grande revisão da aplicação

Para recomeçar a linha de pensamento, recomecei toda a aplicação. O funcionamento da API manteve-se o mesmo, mas alterei o que era relacionado ao Dockerfile, docker-compose.yml, appsettings.json e launchSettings.json.

**Primeiramente** Utilizei Docker para verificar uma possível falha na tecnologia podman. Estava errado, mas a ação me ajudou a compreender melhor a ferramenta de containerização em si.

- Anteriormente, estava usando podman diretamente no terminal Windows [powershell], o que me obrigava à criação de uma máquina virtual.
- Descobri que poderia usar WSL (uma máquina virtual por si próprio), o que me facilitou a interação com os containers.

Duas coisas:
- Tive de adicionar "Kestrel" ao meu appsettings.json. Sinceramente, não tenho certeza de sua importância verdadeira agora.
- O que os containers precisam é de uma NETWORK para comunicarem-se entre si. A NETWORK pode ser criada de duas maneiras [principalmente] (vamos chamar a rede de exemplo de "mynet"):
>`% docker network create --driver=bridge mynet`
>Após a criação dos containers, adicioná-los à rede com: <br>
>`% docker network connect mynet <container_name>`
Ou <br>
Utilizando docker compose, uma rede `default` será criada automaticamente, na qual ambos os containers no docker-compose.yml serão adicionados.

**Além disso** em appsettings.json, tive de alterar a ConnectionURI do mongo para: "mongodb://mongo:27017" (no lugar de localhost), sendo "mongo" o nome do `service`.

### Ou seja
1. Instalar WSL, além de uma Distribution (Ubuntu, Fedora CoreOS, Debian)
2. Instalar podman, seguido de podman-compose (sudo apt install podman-compose) (independente da ordem)
3. Na pasta TaskTrackProject.Api, executar o comando `podman-compose up`
- Ambos os containers do docker-compose.yml serão inicializados, além da network!
- Para testar a conexão: `curl http://localhost:2140/api/Task`, por exemplo, considerando:
> 2140: Porta mapeada localmente para a API (atualmente 6060 no Dockerfile)
> api/Task: Rota para obter valores do banco (também rodando em outro container), como definido em appsettings.json


## CONSOLE

Select-item approach:
https://dev.to/shibayan/how-to-build-an-interactive-command-line-application-in-net-4oic