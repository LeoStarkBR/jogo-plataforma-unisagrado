# Atividade: plataforma temporizada

Abra `Scenes/PlataformaTemporizada.unity` e pressione Play, ou escolha **Desafio da plataforma** no menu principal.

Setas esquerda/direita movem; seta para cima pula; R reinicia; Esc volta ao menu. Saia da ilha inicial, toque na plataforma azul e use sua travessia para alcançar a ilha verde. O painel mostra o estado e o tempo restante. Após cair, reinicie para tentar novamente.

## Requisitos implementados
1. Collider sólido e Rigidbody2D inicialmente Kinematic, com gravidade zero e velocidade zero.
2. Só uma colisão física com um objeto que tenha PlayerMovement ativa a plataforma.
3. Movimento em linha reta para a direita a 3 unidades/s, usando Rigidbody2D.MovePosition no FixedUpdate.
4. Duração fixa de 5 segundos de tempo de física, sem depender da taxa de renderização. O último passo é limitado ao tempo restante; com o passo padrão de 0,02 s, são 250 passos e 15 unidades de percurso.
5. Ao terminar, passa a Dynamic, zera a velocidade, ativa gravidade e bloqueia X para impedir movimento horizontal durante a queda.
6. Estados Waiting → Moving → Falling. Não há transição de volta; outros contatos não reiniciam o movimento. Uma nova tentativa exige reiniciar a cena.

O prefab `Prefabs/PlataformaAtivada.prefab` contém collider, Rigidbody2D e ActivatedPlatform. Ajuste Direction, Speed e Falling Gravity no Inspector. A duração é uma constante de 5 segundos, conforme o enunciado.

A nova cena reutiliza sprites, animações, controles e áudio do projeto. Não contém câmera perseguidora nem geração de obstáculos do runner.

## Validação
O script ActivatedPlatform foi testado em uma cena isolada no Unity 6000.3.25f1, com colisões e simulação física reais. Foram verificados: imobilidade antes do contato, ignorar objetos sem PlayerMovement, ativação pelo jogador, velocidade de 3 unidades/s, duração de 5 segundos, percurso de 15 unidades, mudança para Dynamic com gravidade, bloqueio horizontal na queda e ausência de reativação em contatos posteriores. Os scripts do projeto também passaram na compilação C#. A apresentação visual e a travessia completa ainda devem ser conferidas na cena final.
