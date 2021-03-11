# StorytellingConsoleAdventures
Projeto teste de adventure game em console usando storytelling (ou pelo menos vai usar no futuro...)

VERSÃO 1.0!

Conforme requisitado, o jogo é uma aventura de linha de comando. Ele é representado e jogado todo no console. No jogo, o herói se encontra num cenário onde é perseguido lentamente por um inimigo. O objetivo é encontrar uma espada e derrotá-lo.

Sobre os requisitos:
  ● O jogo deve ter um cenário composto de salas interconectadas; (OK)
  ● O jogador deve ser capaz de:
    ○ Mover-se entre as salas (comandos north, south, west, east); (OK)
    ○ Procurar por itens nas salas (search); (OK)
    ○ Carregar itens encontrados (get nome_do_item); (OK)
    ○ Usar itens carregados (use nome_do_item); (OK)
        Para as próximas iterações ainda vou colocar um comando "help" pra retornar os comandos disponíveis em um dado momento
  ● O jogador deve possuir pontos de vida; (OK)
  ● O jogador deve ser avisado de que sente a presença do inimigo sempre que estiver numa sala adjacente a do inimigo (You feel an evil presence nearby); (OK) 
  ● Uma das salas deve conter o item espada; (OK)
  ● Uma vez na mesma sala que o inimigo, o jogador deve ser avisado de sua presença. Caso não tenha o item espada, a única ação que ele pode fazer é fugir (north, east, west, south). Após fugir, perde 1 ponto de vida; (OK)
  ● Caso seus pontos de vida cheguem a zero o jogador perde o jogo; (OK)
  ● Para matar o inimigo, o jogador deve conseguir usar o item espada quando estiverem na mesma sala; (OK)
  ● O inimigo se move aleatoriamente uma vez a cada 2 movimentos (troca de sala) do jogador. (OK - foi implementado esse comportamento e também o de perseguição)
  
Bônus:
  ● Inimigo persegue o jogador ao invés de se mover aleatoriamente; (OK)
  ● Uma das salas conter um item chave; (OK)
  ● O item espada estar numa sala trancada; (OK)
  ● A sala precisar de um item chave para ser aberta; (OK)
  ● Save / Load game. (OK)
    Os comandos são "save" e "load" respectivamente

Algumas considerações sobre o estado atual:
   Para as próximas iterações ainda vou colocar um comando "help" pra retornar os comandos disponíveis em um dado momento
   O Save e o Load não foram otimizados para salvar e carregar apenas as informações dinâmicas. No momento eles carregam o mapa inteiro
   O Comando de Save não pergunta se você quer salvar por cima de um save antigo, ele simplesmente salva
   É possível ligar/desligar informações que mostram os movimentos e a posição atual do monstro usando a constante DEBUG (que se encontra no arquivo Constants.cs)
   Gostaria de ter feito um editor de mapas também pra colocar mapas em arquivos separados, mas infelizmente não consegui nem começar. O código vem com uma função que cria um mapa para que ele possa ser testado. O nome da função é "InitializeTestScenario" e ela está dentro do GameController.cs
   
   
