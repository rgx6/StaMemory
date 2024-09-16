<script>
    import { onMount } from "svelte";
    import { seasonList } from "../lib/store.js";
    import { api } from "../lib/shared.svelte";

    let isProcessing = false;
    let rankingList = [];

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

    {#each rankingList as ranking}
        <div class="block mt-6">
            <p>{ranking.difficultyName}</p>
            <table class="table">
                <thead>
                    <tr>
                        <th>なまえ</th>
                        <th>たーん</th>
                        <th>にちじ</th>
                    </tr>
                </thead>
                <tbody>
                    {#each ranking.scoreList as score}
                        <tr>
                            <td class="has-text-left">{score.playerName}</td>
                            <td class="has-text-right">{score.turn}</td>
                            <td class="has-text-left">{score.dateTime}</td>
                        </tr>
                    {/each}
                </tbody>
            </table>
        </div>
    {/each}
</div>
