<script>
    import { onMount } from "svelte";
    import { Link } from "svelte-routing";
    import { playerName, defaultPlayerName } from "../lib/store.js";
    import shared from "../lib/shared.js";

    let isProcessing = false;
    let isNameEdit = false;

    onMount(async () => {
        // console.debug("onMount @ home");
    });

    async function updatePlayer() {
        isProcessing = true;

        // @ts-ignore
        const inputValue = document.querySelector("#nameInput").value;

        await shared.api.player.put(inputValue);

        playerName.set(inputValue);

        isNameEdit = false;

        isProcessing = false;
    }
</script>

<div>
    <div class="hero has-text-centered">
        <div class="hero-body">
            <p class="title">しんけいすいじゃく</p>
        </div>
    </div>
    <div class="column is-three-fifths is-offset-one-fifth">
        <div class="block mt-6">
            <Link class="button is-info is-fullwidth" to="/game">げーむ</Link>
        </div>
        <div class="block mt-6">
            <Link class="button is-info is-fullwidth" to="/ranking">らんきんぐ</Link>
        </div>
        <div class="block mt-6">
            <button class="button is-info is-fullwidth" on:click="{() => (isNameEdit = !isNameEdit)}">なまえへんこう</button>
        </div>
        {#if isNameEdit}
            <div class="block mt-6">
                <div class="field is-grouped">
                    <div class="control is-expanded">
                        <input class="input" disabled="{isProcessing}" id="nameInput" type="text" placeholder="{$defaultPlayerName}" value="{$playerName}" maxlength="32" />
                    </div>
                    <div class="control">
                        <button class="button is-info" disabled="{isProcessing}" on:click="{async () => await updatePlayer()}">へんこう</button>
                    </div>
                </div>
            </div>
        {/if}
    </div>
</div>
