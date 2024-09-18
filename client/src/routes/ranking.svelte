<script>
    import { onMount } from "svelte";
    import { seasonList } from "../lib/store.js";
    import { api } from "../lib/shared.svelte";

    let isProcessing = false;
    let rankingList = [];
    let selectedTab = 0;

    onMount(async () => {
        // console.debug("onMount @ ranking");

        // @ts-ignore
        let selectedSeasonId = document.querySelector("#rankingSeason").value;

        rankingList = await api.ranking.get(selectedSeasonId);
    });

    async function changeSeason() {
        isProcessing = true;

        // @ts-ignore
        let selectedSeasonId = document.querySelector("#rankingSeason").value;

        rankingList = await api.ranking.get(selectedSeasonId);

        isProcessing = false;
    }
</script>

<div class="has-text-centered">
    <div class="select">
        <select id="rankingSeason" disabled="{isProcessing}" on:change="{changeSeason}">
            {#each $seasonList as season}
                <option value="{season.seasonId}">{season.seasonName}</option>
            {/each}
        </select>
    </div>

    <div class="tabs is-centered is-boxed mt-6">
        <ul>
            {#each rankingList as ranking, i}
                <!-- svelte-ignore a11y-click-events-have-key-events -->
                <!-- svelte-ignore a11y-no-noninteractive-element-interactions -->
                <li on:click="{() => (selectedTab = i)}" class:is-active="{selectedTab == i}">
                    <!-- svelte-ignore a11y-missing-attribute -->
                    <a>{ranking.difficultyName}</a>
                </li>
            {/each}
        </ul>
    </div>

    {#each rankingList as ranking, i}
        {#if selectedTab == i}
            <div class="block mt-6">
                <table class="table">
                    <thead>
                        <tr>
                            <th></th>
                            <th></th>
                            <th>たーん</th>
                            <th>たいむ</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {#each ranking.scoreList as score, i}
                            <tr>
                                <td class="has-text-right">{i + 1}</td>
                                <td class="has-text-left">{score.playerName}</td>
                                <td class="has-text-right">{score.turn}</td>
                                <td class="has-text-right">{score.clearTime}</td>
                                <td class="has-text-left">{score.clearedAt}</td>
                            </tr>
                        {/each}
                    </tbody>
                </table>
            </div>
        {/if}
    {/each}
</div>
