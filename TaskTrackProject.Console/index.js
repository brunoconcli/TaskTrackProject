const axios = require('axios');
const url = "http://172.18.0.3:6060/api/Task";

axios ({method: 'get', url: url}).then(res => {
  const tasks = res.data;
  tasks.map((task) => {
    const checkbox = task['completed'] ? '[X]' : '[ ]';
    console.log(`${checkbox} ${task['description']}`);
  })
}).catch(err => {
  console.log(err);
})