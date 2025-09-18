<script setup lang="ts">
import {onMounted, ref} from 'vue'
import {TasksApi, type TaskEntity} from './api'

const incompletedItems = ref<TaskEntity[]>([])
const completedItems = ref<TaskEntity[]>([])
const title = ref('')

async function load() {
  incompletedItems.value = await TasksApi.listIncompleted()
  completedItems.value = await TasksApi.listCompleted();
}

async function add() {
  const t = title.value.trim()
  if (!t) return
  await TasksApi.add(t)
  title.value = ''
  await load()
}

async function toggle(item: TaskEntity) {
  await TasksApi.update({id: item.id, title: item.title, isCompleted: !item.isCompleted})
  await load()
}

async function removeItem(id: number) {
  await TasksApi.remove(id)
  await load()
}

onMounted(load)
</script>

<template>
  <div class="p-4">

    <div style="display:flex; gap:8px; margin-bottom:12px;">
      <input class="form-control" v-model="title" placeholder="New task title..."/>
      <button type="button" class="btn btn-primary" @click="add">Add</button>
    </div>

    <h5 class="m-3">Incompleted task</h5>

    <ul class="container">
      <li v-for="t in incompletedItems" :key="t.id">
        <div class="card">
          <div class="card-body task">
            <input type="checkbox" class="form-check-input" :checked="t.isCompleted" @change="() => toggle(t)"/>
            <span>
              {{ t.title ?? '(no title)' }}
            </span>
            <button type="button" class="btn btn-danger" @click="removeItem(t.id)">Delete</button>
          </div>
        </div>
      </li>
    </ul>

    <h5 class="m-3">Completed task</h5>

    <ul class="container">
      <li v-for="t in completedItems" :key="t.id">
        <div class="card">
          <div class="card-body task">
            <input type="checkbox" class="form-check-input" :checked="t.isCompleted" @change="() => toggle(t)"/>
            <span :style="{ textDecoration: 'line-through'}">
              {{ t.title ?? '(no title)' }}
            </span>
            <button type="button" class="btn btn-danger" @click="removeItem(t.id)">Delete</button>
          </div>
        </div>
      </li>
    </ul>

  </div>
</template>

<style scoped>
/* minimal */
</style>
