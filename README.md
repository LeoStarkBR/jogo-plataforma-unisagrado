# Jogo de plataforma 2D — Unisagrado

Projeto individual de um jogo de sobrevivência desenvolvido em Unity com C#.

## Executar
Abra esta pasta pelo Unity Hub com Unity **6000.3.25f1**, abra `Assets/Game/Scenes/MenuPrincipal.unity` e pressione Play no editor. Clique no botão Jogar do menu.
As três cenas já estão habilitadas na ordem correta em Build Profiles. Para gerar um executável, selecione a plataforma e use Build.

## Controles e objetivo
- Setas esquerda/direita: mover no chão e no ar (até 7 unidades por segundo).
- Seta para cima: pular (aproximadamente 1,8 segundo no ar ao voltar à mesma altura, sem usar a seta para baixo).
- Seta para baixo: acelerar a descida.
- Botão Pausar/Continuar: suspender ou retomar a partida.

Avance, evite os espinhos e a parede que vem atrás e colete moedas. A pontuação usa a maior distância horizontal alcançada mais 10 pontos por moeda. Ao morrer, aparecem a pontuação final e o recorde salvo localmente; use Jogar novamente para tentar novamente.

A câmera acompanha o jogador quando ele chega a 68% da largura da tela, deixando espaço à frente para enxergar obstáculos. A parede não avança automaticamente nos primeiros 5 segundos, mas acompanha a câmera caso o jogador avance até essa região. Depois a velocidade mínima começa em 0,3 unidade por segundo. A partir de 20 segundos, aumenta 0,1 unidade por segundo a cada segundo, sem limite: aos 60 segundos são 4,3 e aos 90 segundos são 7,3 unidades por segundo. A velocidade máxima do jogador aumenta automaticamente para manter pelo menos 2 unidades por segundo de vantagem sobre a velocidade automática do cenário. O fundo acompanha a velocidade da câmera com efeito de parallax. Ajuste os valores no GameManager da Main Camera na cena Partida.

## Obstáculos progressivos
Após os primeiros 8 segundos, o jogo gera uma sequência de espinhos à frente da tela. O espaçamento diminui de 24 para aproximadamente 14,5 unidades ao longo de 75 segundos, exigindo saltos cada vez mais frequentes. Mais adiante surgem pares de espinhos, atravessáveis em um único salto. O gerador reserva distância para um salto completo e aterrissagem, e mantém os obstáculos em ordem para evitar sobreposições aleatórias.

## Conceitos aplicados
Entradas por teclado e mouse, Rigidbody2D e colisões, movimentação por Transform, geração de chão e obstáculos, coleta de moedas, pontuação, pausa, troca de cenas, interface e áudio.

## Entrega
Inclua Assets (com os arquivos .meta), Packages e ProjectSettings no ZIP ou repositório. Exclua Library, Temp, Logs e obj. Os recursos gráficos e sonoros de terceiros permanecem com seus arquivos de licença e créditos disponíveis nas pastas de assets.

## Organização
- `Assets/Game/Scenes`: menu, partida e fim de jogo.
- `Assets/Game/Scripts`: controles e sistemas do jogo.
- `Assets/Game/Prefabs`: chão, moedas e espinhos utilizados.
- `Assets/Game/Sprites`, `Audio` e `Materials`: recursos visuais e sonoros utilizados, com licenças preservadas.
- `Assets/TextMesh Pro`: recursos de interface e fontes.
- `Packages` e `ProjectSettings`: dependências e configuração do Unity.

## Trocas de cenário
A cada 250 pontos (distância mais bônus das moedas), o fundo muda com uma mistura gradual de 3 segundos: cenário original → céu com nuvens → litoral → aurora. Aos 1000 pontos retorna ao original e o ciclo continua. A troca preserva o deslocamento do fundo e reinicia com o cenário original em cada partida. Configure `Points Per Background` e `Backgrounds` no componente Scrolling do objeto ScrollingBKG para ajustar marcos e sequência. Os fundos adicionais são do mesmo pacote Craftpix usado pelo projeto; a licença está preservada na pasta de sprites.

## Animações
O personagem alterna entre parado (2 quadros), corrida (4 quadros) e poses de subida, topo e descida do salto (3 quadros), virando para a direção do movimento. As moedas giram usando os 4 quadros da sua categoria original. As animações usam os recortes existentes das folhas de sprites, respeitam a pausa e não alteram os colliders. Espinhos e chão são estáticos, pois suas sprites não têm sequência de animação. Ajuste as velocidades nos componentes PlayerSpriteAnimator e SpriteLoopAnimator.

A aceleração do jogador acompanha a velocidade disponível. O espaçamento dos obstáculos também considera a velocidade futura do jogador para reservar um ciclo de pulo e aterrissagem. A transição de fundo respeita a pausa e mantém o deslocamento das duas imagens sincronizado.

## Segunda atividade: plataforma temporizada
Escolha **Desafio da plataforma** no menu ou abra `Assets/Game/PlatformActivity/Scenes/PlataformaTemporizada.unity`. A plataforma azul só ativa por contato com o jogador, anda por 5 segundos e depois cai sem deslocamento horizontal. Instruções e requisitos detalhados em `Assets/Game/PlatformActivity/README.md`.
