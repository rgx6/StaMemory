import { readable, writable } from "svelte/store";

const _apiUrlBase = import.meta.env.VITE_API_URL_BASE;

export const apiUrlBase = readable(_apiUrlBase);
export const defaultPlayerName = readable("ななしのすたちゅー");

export const playerId = writable(null);
export const playerName = writable(null);
export const difficultyList = writable(null);
export const seasonList = writable(null);
export const game = writable(null);
export const firstCard = writable(null);
export const secondCard = writable(null);
export const isInitialized = writable(false);
export const isError = writable(false);
