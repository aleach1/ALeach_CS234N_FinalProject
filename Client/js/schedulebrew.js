class SchedulePage {

  constructor() {
    this.batch = {
      batchId: "",
      ingredients: []
    };

    this.batches = [];

    // instance variables that the app needs but are not part of the "state" of the application
    this.server = "https://localhost:5000/api"
    this.url = this.server + "/batches";

    // instance variables related to ui elements simplifies code in other places
    this.$batchDisplay = document.querySelector('#batchDisplay');
    this.$form = document.querySelector('#batchForm');
    this.$batchId = document.querySelector('#batchId');
    this.$recipeId = document.querySelector('#recipeId');
    this.$batchSize = document.querySelector('#batchSize');
    this.$scheduleDate = document.querySelector('#scheduleDate');
    this.$deleteButton = document.querySelector('#deleteBtn');
    this.$saveButton = document.querySelector('#saveBtn');

    // call these methods to set up the page
    this.bindAllMethods();
    this.fetchIngredients();

  }

  // any method that is used as part of an event handler must bind this to the class
  bindAllMethods() {
    this.onDeleteBatch = this.onDeleteBatch.bind(this);
    this.onSaveBatch = this.onSaveBatch.bind(this);

    this.fetchIngredients = this.fetchIngredients.bind(this);
    this.fetchBatches = this.fetchBatches.bind(this);
    this.loadBatches = this.loadBatches.bind(this);
    this.clearBatchFields = this.clearBatchFields.bind(this);

    this.$deleteButton.onclick = this.onDeleteBatch;
    this.$saveButton.onclick = this.onSaveBatch;
  }

  // makes an api call to /api/ingredients to get the list of ingredients
  fetchIngredients() {
    fetch(`${this.server}/ingredients`) //this line sends a get request to all ingredients
    .then(response => response.json())
    .then(data => { 
      if (data.length == 0) { //This line makes sure that there is a returned value
        alert("Can't load ingredients.  Can not display brews without ingredient information.");
      }
      else {
        this.batch.ingredients = data; //Fills the ingredients property with the correct data
        this.fetchBatches();
      }
    })
    .catch(error => {
      alert('There was a problem getting ingredient info!'); 
    });
  }

  //makes a call to api/batches to get the list of batches
  fetchBatches() {
    fetch(`${this.url}`) //sends a get request to get batches
      .then(response => response.json())
      .then(data => {
        this.batches = data;
        this.loadBatches(data);
      })
      .catch(error => {
        console.trace();
        alert("There was an error retrieving the batches.");
      })
  }

  // creates a display for each of the batches returned from the api call
  loadBatches() {
    let batchHtml = ``;//this.batches.reduce(
      //(html, thisBatch, index) => html += this.loadBatch(thisBatch, index)
    //);
    for (let i = 0; i < this.batches.length; i++){
      batchHtml += this.loadBatch(this.batches[i]);
    }
    this.$batchDisplay.innerHTML = batchHtml;
   }


  // creates the info for one batch and compares for needed ingredients
  loadBatch(thisBatch) {
    let neededIngredients = "";
    for (let i = 0; i < thisBatch.recipeIngredients.length; i++){
      if (thisBatch.recipeIngredients[i].ingredientId.quantity > this.batch.ingredients[thisBatch.recipeIngredients[i].ingredientId - 1].OnHandQuantity){
        neededIngredients += this.batch.ingredients[thisBatch.recipeIngredients[i].ingredientId - 1].name + "; ";
      }
    }
    if (neededIngredients == ""){
      neededIngredients = "All ingredients in stock."
    }
    return `
      <tr>
        <th scope="row">${thisBatch.batchId}</th>
        <td>${thisBatch.name}</td>
        <td>${thisBatch.volume}</td>
        <td>${neededIngredients}</td>
        <td>${thisBatch.scheduledStartDate}</td>
      </tr>
    `
  }


  // makes a delete request to /api/batches/# where # is the primary key of the batch
  // deletes the batch specified in the form from the database
  onDeleteBatch(event) {
    event.preventDefault();
    if (this.$batchId.value != "") {
      fetch(`${this.url}/${this.$batchId.value}`, {method: 'DELETE'})
      .then(response => response.json())
      .then(data => { 
          this.batch.batchId = data.batchId;
          this.clearBatchFields();
          this.fetchBatches();
          alert("Batch was deleted." + JSON.stringify(data))
        
      })
    }
    else {
      // this should never happen if the right buttons are enabled
    }
  }

  // makes a post request to /api/batches
  // either adds a new batch or updates an existing batch in the database
  // based on the batch information in the form
  onSaveBatch(event) {
    event.preventDefault();
      fetch(`${this.url}`, { //sends post request to api
        method: 'POST', 
        body: JSON.stringify({ //converts js to string
          batchId: this.$batchId.value,
          recipeId: this.$recipeId.value,
          equipmentId: 1,
          volume: parseInt(this.$batchSize.value),
          scheduledStartDate: this.$scheduleDate.value,
          startDate: "2024-12-09T03:41:35.799Z",
          estimatedFinishDate: "2024-12-09T03:41:35.799Z",
          finishDate: "2024-12-09T03:41:35.799Z",
          unitCost: 0,
          notes: "string",
          tasteNotes: "string",
          tasteRating: 0,
          og: 0,
          fg: 0,
          carbonation: 0,
          fermentationStages: 0,
          primaryAge: 0,
          primaryTemp: 0,
          secondaryAge: 0,
          secondaryTemp: 0,
          tertiaryAge: 0,
          age: 0,
          temp: 0,
          ibu: 0,
          ibuMethod: "string",
          abv: 0,
          actualEfficiency: 0,
          calories: 0,
          carbonationUsed: "string",
          forcedCarbonation: 0,
          kegPrimingFactor: 0,
          carbonationTemp: 0,
          equipment: null,
          recipe: null,
          batchContainers: [],
          ingredientInventorySubtractions: [],
          inventoryTransactions: [],
          products: []
        }),
        headers: {
          'Content-Type': 'application/json'
        }
      })
      .then(response => response.json())
      .then(data => { 
        // returns the record that we added so the ids should be there 
        if (data.batchId)
        {
          this.batch.batchId = data.batchId;
          this.fetchBatches();
          this.clearBatchFields();
          alert("The batch was added!");
        }
        else{
          alert("There was a problem adding batch info!"); 
        }
      })
      .catch(error => {
        alert('There was a problem retrieving batch info!'); 
      });
    
  }




 

  

  // clears the ui
  clearBatchFields() {
    this.$batchId.value = "";
    this.$batchSize.value = "";
    this.$scheduleDate.value = "";
  }




}

// instantiate the js app when the html page has finished loading
window.addEventListener("load", () => new SchedulePage());
