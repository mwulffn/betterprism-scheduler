<template>
        <div class="card my-2">
          <div class="card-header d-flex flex-row align-items-center justify-content-between">
            <h6 class="m-0 font-weight-bold text-primary">{{title}}</h6>
          </div>
          <div class="card-body">
            <table class="table table-sm">
              <thead>
                <tr>
                  <th style="width: 40%">Workblock</th>
                  <th style="width: 40%">Process</th>
                  <th>PCD</th>
                  <th>&nbsp;</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in workblocks" v-bind:key="item.WorkblockId">
                  <td>{{item.Name}}</td>
                  <td>{{item.ProcessName}}</td>
                  <td>{{item.PostCompletionDelay}}</td>
                  <td>
                    <router-link :to="{ name: 'workblocks-edit', params: { id: item.WorkblockId} }">
                      <font-awesome-icon icon="fa-edit" />
                    </router-link>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
</template>
<script>
export default { 
  name: "WorkblockList",
  props: {
    title: {
      type: String,
      default: "Hello? Did you forget?"
    },
    workblocks: {
      type: Array,
      default: () => []
    }
  }
};
</script>