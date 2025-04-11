À primeira vez que o programa é executado, serão executadas 3 funções. 

A primeira será a criação de uma coluna de bits, cdu_disponivelPortal,na tabela dbo.whotestadoquartos.
Esta coluna serve para filtrar quais estados é que são apresentados no dropdown dos estados na página dos quartos.

A segunda será as migrações da ASP.NET, para criar as tabelas necessárias para a autenticação do sistema.
Pode se mudar qual será a base de dados escolhida na appsettings.json.

A terceira será o seed das roles nas tabelas da ASP.NET. Serão criadas 4 roles: 
	Admin - com acesso a tudo e ao REGISTO de novos usuários e escolha das suas roles.
	Quartos - só pode aceder à página dos quartos
	Check-Ins - só pode aceder à página dos checkins
	Refeições - só pode aceder à pagina das refeições

Também é criado uma conta admin@admin.com de password: Teste123