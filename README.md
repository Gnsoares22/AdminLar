# AdminLar
Solução criada para administração de condomínios

![Sem título](https://user-images.githubusercontent.com/43541457/89128786-7e90ed00-d4ce-11ea-9416-a5683ffbeb8e.png)

<h1> :computer: Descrição </h1> 

Já pensou em um sistema de administração top para gerenciar seu condomínio. Vem de AdminLar e turbine seu apto !!!. :smiley: :smiley:

<p> AdminLar é um sistema para controle e gerenciamento de condomínios </p>
<p> Este projeto foi criado durante a faculdade na disciplina Programação Web utilizando ASP.NET MVC 5 com Entity Framework </p>

<p> Neste projeto, foram utilizadas as seguintes ferramentas </p>

<ul>
  <li> Entity Framework MySQL </li>
  <li> Template AdminLTE 2.0 </li>
  <li> Linguagens usadas para o desenvolvimento: C# com ASP.NET MVC 5 (.Net framework 4.5.2) </li>
  <li> Os scripts do Bootstrap por padrão já vem embutidos dentro do projeto, mas você pode atualizá-los basta
  ir no Nuget Package Manager >  Nuget Package Manager for Solution > Installed (procurar o bootstrap e atualizar para sua versão mais nova)
</ul>


<h1> :gear: Executando as models com Entity Framework </h1>

<p> Para migrar as tabelas com entity framework para o Mysql, execute os seguintes comandos no terminal (ctrl + ' <- ativa o termina integrado no 
Visual Studio (Developer PowerShell)) : </p>

```
 1° Passo -  PS C:\User\Gabriel\source\repos\AdminLar: Enable-Migrations (exemplo de caminho padrão no meu terminal)
 2° Passo -  PS C:\User\Gabriel\source\repos\AdminLar: Add-Migration nome_classe_model (coloque o nome da model ex: atas)
 3° Passo -  PS C:\User\Gabriel\source\repos\AdminLar: Update-Database (vá no seu mysql e atualize a sessão das tabelas, 
 você verá que a migration adicionada estará lá)
 
 Se der erro, certifique que você instalou o "MySQL.Data.Entities" no Nuget Package Manager e que ela 
 está presente nas References do seu projeto.
 
```

<h1> :white_check_mark: Considerações Finais </h1>

No mais com imensa satisfação deixo este projeto como um marco na minha vida de desenvolvedor !!!.

Feito com muito :heart: e carinho por Gabriel Nascimento Soares
