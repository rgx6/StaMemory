<script context="module">
    import { get } from "svelte/store";
    import { apiUrlBase, playerId, game } from "./store.js";

    export const api = {
        difficulty: {
            get: async function () {
                const apiUrl = get(apiUrlBase) + "/api/difficulty";

                const response = await fetch(apiUrl);

                if (!response.ok) {
                    throw new Error(`response status: ${response.status}`);
                }

                const json = await response.json();

                return json.difficultyList;
            },
        },
        game: {
            create: async function (difficultyId) {
                const apiUrl = get(apiUrlBase) + "/api/game";

                const response = await fetch(apiUrl, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        PlayerId: get(playerId),
                    },
                    body: JSON.stringify({
                        difficultyId: difficultyId,
                    }),
                });

                if (!response.ok) {
                    throw new Error(`response status: ${response.status}`);
                }
            },
            delete: async function () {
                const apiUrl = get(apiUrlBase) + "/api/game";

                const response = await fetch(apiUrl, {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                        PlayerId: get(playerId),
                    },
                    body: JSON.stringify({
                        token: get(game).token,
                    }),
                });

                if (!response.ok) {
                    throw new Error(`response status: ${response.status}`);
                }
            },
            get: async function () {
                const apiUrl = get(apiUrlBase) + "/api/game";

                const response = await fetch(apiUrl, {
                    headers: {
                        PlayerId: get(playerId),
                    },
                });

                if (response.status === 404) {
                    return null;
                } else if (!response.ok) {
                    throw new Error(`response status: ${response.status}`);
                }

                const json = await response.json();

                return json;
            },
            update: async function (cardIndex) {
                const apiUrl = get(apiUrlBase) + "/api/game";

                const response = await fetch(apiUrl, {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                        PlayerId: get(playerId),
                    },
                    body: JSON.stringify({
                        token: get(game).token,
                        cardIndex: cardIndex,
                    }),
                });

                if (!response.ok) {
                    throw new Error(`response status: ${response.status}`);
                }

                const json = await response.json();

                return json;
            },
        },
        player: {
            get: async function () {
                const apiUrl = get(apiUrlBase) + "/api/player";

                const response = await fetch(apiUrl, {
                    headers: {
                        PlayerId: get(playerId),
                    },
                });

                if (!response.ok) {
                    throw new Error(`response status: ${response.status}`);
                }

                const json = await response.json();

                return json.playerName;
            },
            post: async function (playerName) {
                const apiUrl = get(apiUrlBase) + "/api/player";

                const response = await fetch(apiUrl, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        playerName: playerName,
                    }),
                });

                if (!response.ok) {
                    throw new Error(`response status: ${response.status}`);
                }

                const json = await response.json();

                return json.playerId;
            },
            put: async function (playerName) {
                const apiUrl = get(apiUrlBase) + "/api/player";

                const response = await fetch(apiUrl, {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                        PlayerId: get(playerId),
                    },
                    body: JSON.stringify({
                        playerName: playerName,
                    }),
                });

                if (!response.ok) {
                    throw new Error(`response status: ${response.status}`);
                }
            },
        },
        ranking: {
            get: async function (seasonId) {
                const apiUrl = get(apiUrlBase) + "/api/ranking";

                const params = { seasonId: seasonId };
                const query = new URLSearchParams(params);

                const response = await fetch(apiUrl + "?" + query);

                if (!response.ok) {
                    throw new Error(`response status: ${response.status}`);
                }

                const json = await response.json();

                return json.rankingList;
            },
        },
        season: {
            get: async function () {
                const apiUrl = get(apiUrlBase) + "/api/season";

                const response = await fetch(apiUrl);

                if (!response.ok) {
                    throw new Error(`response status: ${response.status}`);
                }

                const json = await response.json();

                return json.seasonList;
            },
        },
    };
</script>
