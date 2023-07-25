<template>
    <div class="card shadow mb-4">
        <a class="d-block card-header py-3" role="button" v-on:click="toggle()">
            <div class="row">
                <div class="col-8">
                    <h6 class="m-0 font-weight-bold text-primary">
                        <font-awesome-icon :icon="collapsed ? 'fas fa-caret-right' : 'fas fa-caret-down'" />
                        {{ session.Name }}
                    </h6>
                </div>
                <div class="col-4 text-right">
                    <small>{{ session.Created | shortdate }}</small>
                </div>
            </div>
        </a>
        <div class="" v-show="!collapsed">
            <div class="card-body">
                <div class="row">
                    <a v-bind:href="full(screenshot.ScreenshotId)" target="_blank" v-for="screenshot in screenshots"
                        v-bind:key="screenshot.ScreenshotId" class="col-3 text-center">
                        <img v-bind:src="thumbnail(screenshot.ScreenshotId)" />
                    </a>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import screenshotService from "../services/screenshotservice";
export default {
    name: 'screenshot-session',
    props: {
        session: Object
    },
    data() {
        return {
            screenshots: null,
            collapsed: true
        };
    },
    methods: {
        thumbnail(id) {
            return screenshotService.thumbnail(id);
        },
        full(id) {
            return screenshotService.full(id);
        },
        async toggle() {
            if (this.screenshots === null) {
                this.screenshots = await screenshotService.screenshots(this.session.SessionId);
            }
            this.collapsed = !this.collapsed
        }
    }
}
</script>
<style>
</style>