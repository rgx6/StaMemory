<script>
    import { onMount } from "svelte";
    import { Router, Link, Route } from "svelte-routing";
    import { isInitialized, isError, playerId, playerName, defaultPlayerName, game, difficultyList, seasonList } from "./lib/store.js";
    import { api } from "./lib/shared.svelte";

    import Home from "./routes/home.svelte";
    import Game from "./routes/game.svelte";
    import Ranking from "./routes/ranking.svelte";
    import NotFound from "./routes/notfound.svelte";

    onMount(async () => {
        // console.debug("onMount @ app");

        if ($isInitialized) return;

        let localPlayerId = localStorage.getItem("playerId");

        if (localPlayerId == null || localPlayerId.trim().length !== 32) {
            localPlayerId = await api.player.post($defaultPlayerName);

            localStorage.setItem("playerId", localPlayerId);

            $playerId = localPlayerId;
            $playerName = $defaultPlayerName;
        } else {
            $playerId = localPlayerId;
            $playerName = await api.player.get();
            $game = await api.game.get();
        }

        $difficultyList = await api.difficulty.get();
        $seasonList = await api.season.get();

        $isInitialized = true;
    });

    window.onunhandledrejection = function (event) {
        // console.debug("on unhandledrejection");

        console.error(event);

        $isError = true;
    };

    window.onerror = function (message, url, line, column, error) {
        // console.debug("on error");

        console.error(message, url, line, column, error);

        $isError = true;
    };
</script>

{#if $isError}
    <div class="center has-text-centered">
        <div class="block">
            <p>えらー</p>
        </div>
        <div class="block mt-6">
            <button class="button is-info">
                <a href="/">もどる</a>
            </button>
        </div>
    </div>
{:else if $isInitialized}
    <Router>
        <nav class="navbar" aria-label="main navigation">
            <div class="navbar-menu">
                <div class="navbar-start">
                    <Link class="navbar-item" to="/">たいとる</Link>
                    <Link class="navbar-item" to="/game">あそぶ</Link>
                    <Link class="navbar-item" to="/ranking">らんきんぐ</Link>
                </div>
                <div class="navbar-end">
                    <div class="navbar-item">
                        <div class="navbar-item">{$playerName}</div>
                    </div>
                </div>
            </div>
        </nav>
        <main class="block mt-6 mb-6">
            <Route path="/" component="{Home}" />
            <Route path="/game" component="{Game}" />
            <Route path="/ranking" component="{Ranking}" />
            <Route component="{NotFound}" />
        </main>
    </Router>
{:else}
    <div class="center">
        <span class="icon">
            <i class="loader"></i>
        </span>
    </div>
{/if}
