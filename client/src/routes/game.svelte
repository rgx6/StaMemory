<script>
    import { onDestroy, onMount } from "svelte";
    import { navigate } from "svelte-routing";
    import { get } from "svelte/store";
    import { playerId, difficultyList, game, firstCard, secondCard } from "../lib/store.js";
    import shared from "../lib/shared.js";

    let isCleared = false;
    let isProcessing = false;

    onMount(async () => {
        // console.debug("onMount @ game");

        if (get(playerId) == null || get(playerId).trim() == "") {
            navigate("/", { replace: true });
            return;
        }

        if (get(game) != null && get(game).firstCardIndex != null) {
            const card = {
                cardIndex: get(game).firstCardIndex,
                cardId: get(game).firstCardId,
            };

            firstCard.set(card);

            const button = document.querySelector("#card" + get(game).firstCardIndex);

            setFlippedCardClass(button, get(game).firstCardId);

            return;
        }
    });

    onDestroy(async () => {
        // console.debug("onDestroy @ game");

        if (isCleared) {
            firstCard.set(null);
            secondCard.set(null);
            game.set(null);
        }
    });

    async function createGame(difficultyId) {
        isProcessing = true;

        await shared.api.game.create(difficultyId);
        const _game = await shared.api.game.get();
        game.set(_game);

        isCleared = false;

        isProcessing = false;
    }

    async function deleteGame() {
        isProcessing = true;

        await shared.api.game.delete();

        game.set(null);

        isProcessing = false;
    }

    function retryGame() {
        game.set(null);
    }

    async function flipCard(event, index) {
        isProcessing = true;

        if (get(game).cards[index].isOpen) {
            isProcessing = false;
            return;
        }

        if (get(game).firstCardIndex == index) {
            isProcessing = false;
            return;
        }

        const button = event.currentTarget;

        const updatedGame = await shared.api.game.update(index);

        get(game).token = updatedGame.token;

        if (get(game).firstCardIndex == null) {
            const card = {
                cardIndex: updatedGame.flippedCardIndex,
                cardId: updatedGame.flippedCardId,
            };

            firstCard.set(card);

            get(game).firstCardIndex = updatedGame.flippedCardIndex;
            get(game).firstCardId = updatedGame.flippedCardId;

            setFlippedCardClass(button, updatedGame.flippedCardId);

            isProcessing = false;

            return;
        }

        const card = {
            cardIndex: updatedGame.flippedCardIndex,
            cardId: updatedGame.flippedCardId,
        };

        secondCard.set(card);

        setFlippedCardClass(button, updatedGame.flippedCardId);

        // アニメーション完了待ち
        setTimeout(() => {
            if (updatedGame.isCleared) {
                firstCard.set(null);
                secondCard.set(null);

                isCleared = true;
                isProcessing = false;

                return;
            } else if (updatedGame.isMatched) {
                get(game).cards[$firstCard.cardIndex].cardId = $firstCard.cardId;
                get(game).cards[$firstCard.cardIndex].isOpen = true;
                get(game).cards[$secondCard.cardIndex].cardId = $secondCard.cardId;
                get(game).cards[$secondCard.cardIndex].isOpen = true;

                get(game).firstCardIndex = null;
                get(game).firstCardId = null;

                firstCard.set(null);
                secondCard.set(null);

                get(game).turn = get(game).turn + 1;
                game.set(get(game));

                isProcessing = false;

                return;
            }

            setTimeout(() => {
                resetFlippedCardClass($firstCard.cardIndex, $firstCard.cardId);
                resetFlippedCardClass($secondCard.cardIndex, $secondCard.cardId);

                get(game).firstCardIndex = null;
                get(game).firstCardId = null;

                firstCard.set(null);
                secondCard.set(null);

                get(game).turn = get(game).turn + 1;
                game.set(get(game));

                isProcessing = false;
            }, 500);
        }, 500);
    }

    function setFlippedCardClass(e, cardId) {
        e.querySelectorAll("div").forEach((e) => {
            if (e.classList.contains("front")) {
                e.classList.add("flipped", "sprite-stamp" + cardId);
            } else {
                e.classList.add("flipped");
            }
        });
    }

    function resetFlippedCardClass(cardIndex, cardId) {
        const e = document.querySelector("#card" + cardIndex);
        e.querySelectorAll("div").forEach((e) => {
            if (e.classList.contains("front")) {
                e.classList.remove("flipped");
                setTimeout(() => {
                    e.classList.remove("sprite-stamp" + cardId);
                }, 500);
            } else {
                e.classList.remove("flipped");
            }
        });
    }
</script>

<div class="has-text-centered">
    {#if $game == null}
        <div class="block mt-6 mb-6">
            <p>れべる</p>
        </div>
        {#if $difficultyList != null}
            {#each $difficultyList as difficulty}
                <div class="block">
                    <button class="button is-info" disabled="{isProcessing}" on:click="{async () => await createGame(difficulty.difficultyId)}">{difficulty.difficultyName}</button>
                </div>
            {/each}
        {/if}
    {:else}
        <div class="block mt-6">
            <p class="label">たーん {$game.turn}</p>
        </div>
        <div class="block mt-6">
            {#each $game.cards as card, i}
                {#if card.isOpen}
                    <button id="{'card' + i}" class="card is-shadowless" disabled>
                        <div class="sprite {'sprite-stamp' + card.cardId}"></div>
                    </button>
                {:else}
                    <button id="{'card' + i}" class="card is-shadowless" disabled="{isProcessing}" on:click="{async (e) => await flipCard(e, i)}">
                        <div class="front sprite"></div>
                        <div class="back"></div>
                    </button>
                {/if}
                {#if (i + 1) % $game.width === 0}
                    <br />
                {/if}
            {/each}
        </div>
        <div class="block mt-6">
            {#if isCleared}
                <div class="mt-6">
                    <p>くりあ</p>
                </div>
                <div class="mt-6">
                    <button class="button is-info" on:click="{retryGame}">もういちど</button>
                </div>
            {:else}
                <button class="button is-warning" disabled="{isProcessing}" on:click="{deleteGame}">あきらめる</button>
            {/if}
        </div>
    {/if}
    {#if isProcessing}
        <div class="block mt-6">
            <span class="icon">
                <i class="loader"></i>
            </span>
        </div>
    {/if}
</div>
