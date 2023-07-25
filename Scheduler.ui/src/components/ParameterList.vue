<template>
  <dl v-if="items.length > 0" class="row">
    <template v-for="output in items">
      <dt class="col-3" v-bind:key="output.Name">{{ output.Name }}:</dt>
      <dd class="col-9" v-bind:key="output.Name + 'dd'">
        <div v-if="output.Type == 'collection'">
          <a href="#"
            @click="
              showLog(logId, output.Name);
              $event.preventDefault();
            ">{{ output.Value }}</a
          >
        </div>
        <div v-if="output.Type != 'collection'">
          {{ output.Value }}
        </div>
      </dd>
    </template>
  </dl>
</template>
<script>
export default {
  name: "ParameterList",
  emits: ["showLog"],
  props: {
    items: {
      type: Array,
      default: () => [],
    },
    logId: {
      type: Number,
      default: 0,
    },
  },
  methods: {
    showLog(logId, parameterName) {
      this.$emit("showLog", { logId: logId, name: parameterName });
    },
  },
};
</script>