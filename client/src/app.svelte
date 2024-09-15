<script>
    import { onMount } from "svelte";
    import { Router, Link, Route } from "svelte-routing";
    import { isInitialized, isError } from "./lib/store.js";
    import shared from "./lib/shared.js";

    import Home from "./routes/home.svelte";
    import Game from "./routes/game.svelte";
    import Ranking from "./routes/ranking.svelte";
    import NotFound from "./routes/notfound.svelte";

    onMount(async () => {
        // console.debug("onMount @ app");

        await shared.init();
    });

    window.onunhandledrejection = function (event) {
        // console.debug("on unhandledrejection");

        console.error(event);

        isError.set(true);
    };

    window.onerror = function (message, url, line, column, error) {
        // console.debug("on error");

        console.error(message, url, line, column, error);

        isError.set(true);
    };
</script>

{#if $isError}
    <div class="has-text-centered">
        <div class="block mt-6">
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
                    <Link class="navbar-item" to="/game">げーむ</Link>
                    <Link class="navbar-item" to="/ranking">らんきんぐ</Link>
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
    <div class="has-text-centered">
        <div class="block mt-6">
            <span class="icon">
                <i class="loader"></i>
            </span>
        </div>
    </div>
{/if}
