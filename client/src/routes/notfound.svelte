<script>
    import { onMount } from "svelte";
    import { navigate } from "svelte-routing";

    onMount(async () => {
        // console.debug("onMount @ not found");

        navigate("/", { replace: true });
    });
</script>
