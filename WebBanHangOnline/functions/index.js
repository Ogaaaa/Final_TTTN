const functions = require('firebase-functions');
const { WebhookClient } = require('dialogflow-fulfillment');

const laptops = [
    { id: 1, name: 'Laptop A', price: 1000 },
    { id: 2, name: 'Laptop B', price: 1200 },
    { id: 3, name: 'Laptop C', price: 900 }
];

exports.dialogflowWebhook = functions.https.onRequest((request, response) => {
    const agent = new WebhookClient({ request, response });

    function searchProduct(agent) {
        const product = agent.parameters.product;
        const results = laptops.filter(laptop => laptop.name.toLowerCase().includes(product.toLowerCase()));
        if (results.length > 0) {
            agent.add(`Tôi tìm thấy các sản phẩm sau: ${results.map(laptop => laptop.name).join(', ')}`);
        } else {
            agent.add("Không tìm thấy sản phẩm nào.");
        }
    }

    let intentMap = new Map();
    intentMap.set('SearchProduct', searchProduct);
    agent.handleRequest(intentMap);
});
