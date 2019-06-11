# FileManager
Apresentação de amostragem de um sistema gestão de arquivos

Para iniciar copie a pasta temp para o C:/

Execute dentro da pasta do projeto o comando
  dotnet restore

Agora em seguida execute o comando
  dotnet run
 
Caos não possua o dotnet client, baixe e instale no link abaixo:
  https://dotnet.microsoft.com/download
  
A classe DataGenerator no projeto WebApi.Utils é responsável por gerar os primeiros dados de testes.
Caso necessite adicionar mais arquivos,ou até mesmo atualizar ou deletar, pode-se utilizar a API para esse controle.

  https://localhost:5001/swagger/index.html
  
  Nesse link também está a documentação.
  
Um serviço em background fica a parte de fazer toda movimentação dos arquivos e atualizar seus status e as respectivas Datas:
    InitialDate,
    FinishedDate
    "Estas informações são pertinentes a classe File no namespace FileManager.Core.Models"

O serviço roda de minuto a minuto.

O resultado final pode ser visto na pasta c:/temp/DEST.
