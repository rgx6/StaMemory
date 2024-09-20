<script>
    import { onDestroy, onMount } from "svelte";
    import { navigate } from "svelte-routing";
    import { playerId, difficultyList, game, firstCard, secondCard } from "../lib/store.js";
    import { api } from "../lib/shared.svelte";

    let isCleared = false;
    let isProcessing = false;
    let clearTime = 0;

    onMount(async () => {
        // console.debug("onMount @ game");

        if ($playerId == null || $playerId.trim() === "") {
            navigate("/", { replace: true });
            return;
        }

        if ($game != null && $game.firstCardIndex != null) {
            const card = {
                cardIndex: $game.firstCardIndex,
                cardId: $game.firstCardId,
            };

            $firstCard = card;

            const button = document.querySelector("#card" + $game.firstCardIndex);

            setFlippedCardClass(button, $game.firstCardId);

            return;
        }
    });

    onDestroy(async () => {
        // console.debug("onDestroy @ game");

        if (isCleared) {
            $firstCard = null;
            $secondCard = null;
            $game = null;
        }
    });

    async function createGame(difficultyId) {
        isProcessing = true;

        await api.game.create(difficultyId);
        $game = await api.game.get();

        isCleared = false;

        isProcessing = false;
    }

    async function deleteGame() {
        isProcessing = true;

        await api.game.delete();

        $game = null;

        isProcessing = false;
    }

    function retryGame() {
        $game = null;
    }

    async function flipCard(event, index) {
        isProcessing = true;

        if ($game.cards[index].isOpen) {
            isProcessing = false;
            return;
        }

        if ($game.firstCardIndex == index) {
            isProcessing = false;
            return;
        }

        const button = event.currentTarget;

        const updatedGame = await api.game.update(index);

        $game.token = updatedGame.token;

        if ($game.firstCardIndex == null) {
            const card = {
                cardIndex: updatedGame.flippedCardIndex,
                cardId: updatedGame.flippedCardId,
            };

            $firstCard = card;

            $game.firstCardIndex = updatedGame.flippedCardIndex;
            $game.firstCardId = updatedGame.flippedCardId;

            setFlippedCardClass(button, updatedGame.flippedCardId);

            isProcessing = false;

            return;
        }

        const card = {
            cardIndex: updatedGame.flippedCardIndex,
            cardId: updatedGame.flippedCardId,
        };

        $secondCard = card;

        setFlippedCardClass(button, updatedGame.flippedCardId);

        // アニメーション完了待ち
        setTimeout(() => {
            if (updatedGame.isCleared) {
                $firstCard = null;
                $secondCard = null;

                clearTime = updatedGame.clearTime;

                isCleared = true;
                isProcessing = false;

                return;
            } else if (updatedGame.isMatched) {
                $game.cards[$firstCard.cardIndex].cardId = $firstCard.cardId;
                $game.cards[$firstCard.cardIndex].isOpen = true;
                $game.cards[$secondCard.cardIndex].cardId = $secondCard.cardId;
                $game.cards[$secondCard.cardIndex].isOpen = true;

                $game.firstCardIndex = null;
                $game.firstCardId = null;

                $firstCard = null;
                $secondCard = null;

                $game.turn = $game.turn + 1;
                $game = $game;

                isProcessing = false;

                return;
            }

            setTimeout(() => {
                resetFlippedCardClass($firstCard.cardIndex, $firstCard.cardId);
                resetFlippedCardClass($secondCard.cardIndex, $secondCard.cardId);

                $game.firstCardIndex = null;
                $game.firstCardId = null;

                $firstCard = null;
                $secondCard = null;

                $game.turn = $game.turn + 1;
                $game = $game;

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
            <p>たーん {$game.turn}</p>
        </div>
        <div class="block mt-6">
            {#if isCleared}
                <div class="mt-6">
                    <p>くりあたいむ</p>
                    <p>{clearTime}</p>
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
