const recenterBtn = document.getElementById("cy-recenter");

const removeSuggestionsBtns = document.querySelectorAll(".btn-suggestion-del");

removeSuggestionsBtns.forEach(btn => {
    btn.addEventListener("click", (e) => {
        const listItem = btn.closest("li");

        if (listItem) {
            listItem.style.opacity = '0';
            listItem.style.transform = 'translateX(20px)';
            listItem.style.transition = 'all 0.3s ease';

            setTimeout(() => {
                listItem.remove();
            }, 300);
        }
    });
});

function openRemoveSpecieDialog(btn) {
    const id = btn.dataset.specieSlug;
    const name = btn.dataset.specieName;
    const count = btn.dataset.specieCount;

    document.getElementById('remove-specie-name').textContent = name;
    document.getElementById('remove-specie-count').textContent = count;
    document.getElementById('remove-specie-id-input').value = id;
    document.getElementById('remove-max').textContent = count;
    document.getElementById('remove-count-input').max = count;
    document.getElementById('remove-count-input').value = 1;

    document.getElementById('remove-species-dialog').showModal();
}

function openRemoveEquipmentDialog(btn) {
    const id = btn.dataset.equipmentSlug;
    const name = btn.dataset.equipmentName;
    const count = btn.dataset.equipmentCount;

    document.getElementById('remove-equipment-name').textContent = name;
    document.getElementById('remove-equipment-count').textContent = count;
    document.getElementById('remove-equipment-id-input').value = id;
    document.getElementById('remove-max').textContent = count;
    document.getElementById('remove-count-input').max = count;
    document.getElementById('remove-count-input').value = 1;

    document.getElementById('remove-equipment-dialog').showModal();
}

(function () {
    const analyze = window.data;
    const stats = analyze.Stats;
    
    const data = analyze.Graph;
    
    if (!data) {
        console.error("foodChainData is missing");
        return;
    }

    const nodes = data.Nodes || [];
    const edges = data.Edges || [];

    const elements = [
        ...nodes.map(n => {
            const isStarved = stats.StarvedSpecies.some(s => s.Specie.Slug === n.Specie.Slug);
            const isUnderfed = stats.UnderfedSpecies.some(s => s.Specie.Slug === n.Specie.Slug);

            return {
                data: {
                    id: n.Specie.Slug,
                    label: `${n.Specie.Name}\n${n.Count}`,
                    count: n.Count,
                    hungerStatus: isStarved ? 'starved' : (isUnderfed ? 'underfed' : 'normal')
                }
            };
        }),
        ...edges.map(e => ({
            data: {
                source: e.PredatorSlug,
                target: e.PreySlug
            }
        }))
    ];

    const cy = cytoscape({
        container: document.getElementById('cy'),
        maxZoom: 1.5,
        minZoom: 0.2,
        elements,

        style: [
            {
                selector: 'node',
                style: {
                    'width': function (ele) {
                        const count = ele.data('count') || 0;
                        return Math.min(60 + (count * 4), 140) + 'px';
                    },
                    'height': function (ele) {
                        const count = ele.data('count') || 0;
                        return Math.min(60 + (count * 4), 140) + 'px';
                    },
                    'font-size': function (ele) {
                        const count = ele.data('count') || 0;
                        return Math.min(12 + (count * 0.5), 18) + 'px';
                    },

                    'background-color': '#132830',
                    'label': 'data(label)',
                    'color': '#e8f4f2',
                    'font-weight': '600',
                    'text-valign': 'center',
                    'text-halign': 'center',
                    'text-wrap': 'wrap',
                    'text-max-width': 80,
                    'line-height': 1.2,

                    'border-width': 1.5,
                    'border-color': 'rgba(0, 210, 180, 0.4)',

                    'text-outline-width': 2,
                    'text-outline-color': '#060d0f',

                    'transition-property': 'background-color, border-color, border-width',
                    'transition-duration': '0.2s'
                }
            },

            {
                selector: 'edge',
                style: {
                    'width': 2,
                    'line-color': 'rgba(122, 173, 168, 0.3)',
                    'target-arrow-color': 'rgba(0, 210, 180, 0.4)',
                    'target-arrow-shape': 'triangle',
                    'curve-style': 'bezier',
                    'arrow-scale': 1.2,
                    'opacity': 0.8,
                    'ghost': 'yes',
                    'ghost-offset-x': 0,
                    'ghost-offset-y': 1,
                    'ghost-opacity': 0.1
                }
            },

            {
                selector: 'node[hungerStatus = "underfed"]',
                style: {
                    'border-color': '#ff9800',
                    'border-width': 3,
                    'border-opacity': 1
                }
            },

            {
                selector: 'node[hungerStatus = "starved"]',
                style: {
                    'border-color': '#f44336',
                    'border-width': 4,
                    'border-opacity': 1,
                }
            },

            {
                selector: ':selected',
                style: {
                    'border-color': '#ffffff',
                    'background-color': '#1a3a42'
                }
            },

            {
                selector: 'node[count = 0]',
                style: {
                    'background-color': '#2c2c2c',
                    'border-color': '#555555',
                    'opacity': 0.6,
                    'color': '#888888'
                }
            }
        ],

        layout: {
            name: 'breadthfirst',
            directed: true,
            padding: 40,
            spacingFactor: 1.7,
            animate: false,
        }
    });

    const popper = document.getElementById('node-popper');

    cy.on('tap', 'node', function(evt) {
        const node = evt.target;
        const status = node.data('hungerStatus');
        const name = node.data('label').split('\n')[0];

        if (status === 'normal') {
            popper.style.display = 'none';
            return;
        }

        document.getElementById('tooltip-name').textContent = name;
        const statusEl = document.getElementById('tooltip-status');
        statusEl.textContent = status === 'starved' ? 'Affamé' : 'Sous-alimenté';
        statusEl.className = status === 'starved' ? 'status-starved' : 'status-underfed';

        const cyContainer = document.getElementById('cy');
        const rect = cyContainer.getBoundingClientRect();
        const pos = node.renderedPosition();

        popper.style.display = 'block';
        popper.style.top = (pos.y + rect.top + window.scrollY - 80) + 'px';
        popper.style.left = (pos.x + rect.left + window.scrollX + 10) + 'px';
    });

    cy.on('tap', function(event) {
        if (event.target === cy) {
            popper.style.display = 'none';
        }
    });

    cy.on('pan zoom', () => { popper.style.display = 'none'; });

    function pulseNodes() {
        const starvedNodes = cy.nodes('[hungerStatus = "starved"]');

        starvedNodes.forEach(node => {
            function pulse() {
                node.animation({
                    style: { 'border-opacity': 0.6 },
                    duration: 700
                }).play().promise().then(() => {
                    node.animation({
                        style: { 'border-opacity': 1 },
                        duration: 700
                    }).play().promise().then(pulse);
                });
            }

            pulse();
        });
    }

    recenterBtn.addEventListener("click", (e) => {
        cy.fit(null, 50)
    })

    setTimeout(() => {
        cy.resize();
        cy.fit();
    }, 50);

    window.addEventListener('resize', () => {
        cy.resize();
    });
    
    pulseNodes();
    setTimeout(() => {
        cy.fit(null, 50)
    }, 50)
})();
